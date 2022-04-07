import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserInfo } from '../interfaces/UserInfo';
import { StartLoanRequest } from './requests/StartLoanRequest';
import { StopLoanRequest } from './requests/StopLoanRequest';
import { EnvironmentUrls } from './Environments';


@Injectable({
  providedIn: 'root'
})
export class UserHttpService {
  baseurl: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };
  constructor(private httpClient: HttpClient) {
    this.baseurl = new EnvironmentUrls().isLocal(false) + 'api/users';
  }
  public AddUserInfo(user: UserInfo): Observable<object> {
    return this.httpClient.post(this.baseurl, user, this.httpOptions);
  }
  public GetUser(firebaseUid: string): Observable<object> {
    const url =  `${this.baseurl}/getuserid/${firebaseUid}`;
    return this.httpClient.get(url);
  }
  public GetLoanHistory(id): Observable<object> {
    const url = `${this.baseurl}/loan-history/${id}`;
    return this.httpClient.get(url);
  }
  public StartLoan(start: StartLoanRequest): Observable<object> {
    return this.httpClient.post(`${this.baseurl}/startloan`, start, this.httpOptions);
  }
  public StopLoan(stop: StopLoanRequest): Observable<object> {
    return this.httpClient.post(`${this.baseurl}/stoploan`, stop, this.httpOptions);
  }
}
