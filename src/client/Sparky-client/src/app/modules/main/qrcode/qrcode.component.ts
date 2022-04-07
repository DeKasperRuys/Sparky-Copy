import { Component, Input, OnInit } from '@angular/core';
import { BarcodeFormat } from '@zxing/library';
import { BehaviorSubject } from 'rxjs';
import { ChargingStation } from 'src/app/interfaces/ChargingStations';
import { ChargingstationService } from 'src/app/services/chargingstation.service';
import { CharginstationsHttpService } from 'src/app/http/Chargingstations-http.service';
import { SimpleTimer } from 'ng2-simple-timer';
import { post } from 'selenium-webdriver/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-qrcode',
  templateUrl: './qrcode.component.html',
  styleUrls: ['./qrcode.component.scss']
})
export class QrcodeComponent implements OnInit {

  constructor(
    private csService: ChargingstationService, 
    private csHttpService: CharginstationsHttpService,
    private st: SimpleTimer,
    private router: Router) {
  }

  // Timer
  counter0 = 0;
	timer0Id: string;
	timer0Name: string = '1 sec';
  timer0button = 'Subscribe';
  totalSecondsLent = 0;
  totalMinutsLent = 0;
  totalPrice = 0;
  pricePer15Min = 0.25;

  // HTTP
  stations: ChargingStation[] = [];
  listOfStations = [];
  station: ChargingStation;

  lockBool = true;
  checkBool = false;
  unlocked = true;
  locked = false;

  // Qrcode
  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo = null;
  hasDevices: boolean;
  hasPermission: boolean;
  qrResultString: string;

  torchEnabled = false;
  torchAvailable$ = new BehaviorSubject<boolean>(false);
  tryHarder = false;

  formatsEnabled: BarcodeFormat[] = [
    BarcodeFormat.CODE_128,
    BarcodeFormat.DATA_MATRIX,
    BarcodeFormat.EAN_13,
    BarcodeFormat.QR_CODE,
  ];

 ngOnInit() {
   // get stations
  this.csService.ChargingStations.subscribe(data => {
  this.stations = data;
  console.log(this.stations);
  for (const i in this.stations) {
    this.listOfStations.push(this.stations[i].chargingStationId);
    console.log(this.listOfStations[i]); }} );
}

  updateIsRequested(bool: boolean) {
    this.csHttpService.putLoanRequest(parseInt(this.qrResultString), bool).subscribe();
  }
  clearResult(): void {
    this.qrResultString = null;
  }

  unlock(): void {
   this.lockBool = false;
   this.updateIsRequested(this.unlocked);
  }

  lock(): void {
    this.lockBool = true;
    this.updateIsRequested(this.locked);
  }

  price(totalSec: number, price: number): number {
    const totalMin = totalSec / 60;
    const per15min = totalMin / 15;
    return per15min * price;
  }

  onCamerasFound(devices: MediaDeviceInfo[]): void {
    this.availableDevices = devices;
    this.hasDevices = Boolean(devices && devices.length);
  }

  onCodeResult(resultString: string) {
    this.qrResultString = resultString;
    this.checkBool = this.listOfStations.includes(parseInt(this.qrResultString));
    console.log(this.checkBool);
    this.csService.setAvailableId(parseInt(this.qrResultString));
    this.router.navigate(['/main/startloan']);
  }

  onDeviceSelectChange(selected: string) {
    const device = this.availableDevices.find(x => x.deviceId === selected);
    this.currentDevice = device || null;
  }

  onHasPermission(has: boolean) {
    this.hasPermission = has;
  }

  onTorchCompatible(isCompatible: boolean): void {
    this.torchAvailable$.next(isCompatible || false);
  }

  toggleTorch(): void {
    this.torchEnabled = !this.torchEnabled;
  }

  toggleTryHarder(): void {
    this.tryHarder = !this.tryHarder;
  }




}
