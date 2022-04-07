import { Injectable } from '@angular/core';
import { ChargingStation } from '../interfaces/ChargingStations';
import { BehaviorSubject } from 'rxjs';
import { CharginstationsHttpService } from '../http/Chargingstations-http.service';

@Injectable({
  providedIn: 'root'
})
export class ChargingstationService {
  chargingStations: Array<ChargingStation> = [];
  stationId = 0;
  constructor(private chargingStationHttpService: CharginstationsHttpService) { }
  private source = new BehaviorSubject<Array<ChargingStation>>(this.chargingStations);
  private sourceStationId = new BehaviorSubject<number>(this.stationId);
  ChargingStations = this.source.asObservable();
  currentStationId = this.sourceStationId.asObservable();
  setChargingStations(stations: Array<ChargingStation>) {
    this.source.next(stations);
  }
  setAvailableId(id: number) {
    this.sourceStationId.next(id);
  }

  updateCharginStations() {
    this.chargingStationHttpService.getStations().subscribe((stations: ChargingStation[]) => {
      this.source.next(stations);
      console.log(stations);
    });
  }
}
