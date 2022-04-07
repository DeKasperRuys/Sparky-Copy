import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { ChargingStation } from 'src/app/interfaces/ChargingStations';
import { PowerbankDto } from '../interfaces/PowerbankDto';
import { EnvironmentUrls } from './Environments';

@Injectable({
  providedIn: 'root'
})

export class CharginstationsHttpService {

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  baseUrl: string;
  constructor(private httpclient: HttpClient) {
    this.baseUrl = new EnvironmentUrls().isLocal(false) + 'api/stations';
   }

  public getStations(): Observable<object> {
    return this.httpclient.get(this.baseUrl);
  }

  // loan powerbank
  public putLoanRequest(id, request: boolean): Observable<ChargingStation> {
    const url = `${this.baseUrl}/loanRequest/${id}`;
    return this.httpclient.put<ChargingStation>(url, {setBoolean: request}, this.httpOptions);
  }
  public pollAvailablePowerbank(id): Observable<PowerbankDto> {
    const url = `${this.baseUrl}/poll/availablepowerbank/${id}`;
    return this.httpclient.get<PowerbankDto>(url);
  }
  public putOpenStation(id, open: boolean): Observable<ChargingStation> {
    const url = `${this.baseUrl}/openstation/${id}`;
    return this.httpclient.put<ChargingStation>(url, {setBoolean: open}, this.httpOptions);
  }
  public resetAvailablePowerbank(id) {
    const url = `${this.baseUrl}/availablepowerbank/${id}`;
    return this.httpclient.put<ChargingStation>(url, {id: 0});
  }
  // return powerbank
  public putReturnRequest(id, returnRequest: boolean): Observable<ChargingStation> {
    const url = `${this.baseUrl}/isreturnrequest/${id}`;
    return this.httpclient.put<ChargingStation>(url, {setBoolean: returnRequest}, this.httpOptions);
  }
  public pollReturnOk(id): Observable<object> {
    const url = `${this.baseUrl}/poll/returnOk/${id}`;
    return this.httpclient.get(url);
  }
  public putCloseStation(id, close: boolean): Observable<ChargingStation> {
    const url = `${this.baseUrl}/closeStation/${id}`;
    return this.httpclient.put<ChargingStation>(url, {setBoolean: close}, this.httpOptions);
  }
  public resetReturnOK(id) {
    const url =  `${this.baseUrl}/returnOk/${id}`;
    return this.httpclient.put<ChargingStation>(url, false);
  }
  public updateSlot(id, powerbankId, slotId ) {
    const url =  `${this.baseUrl}/update-slot/${id}`;
    return this.httpclient.put<ChargingStation>(url, {slotId, powerbankId});
  }




}
