import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { ChargingstationService } from 'src/app/services/chargingstation.service';
import { SimpleTimer } from 'ng2-simple-timer';
import { CharginstationsHttpService } from 'src/app/http/Chargingstations-http.service';
import { Observable, timer, interval } from 'rxjs';
import { concatMap, map, tap, startWith, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { PowerbankDto } from 'src/app/interfaces/PowerbankDto';
import { UserHttpService } from 'src/app/http/user-http.service';
import { StartLoanRequest } from 'src/app/http/requests/StartLoanRequest';
import { LoanHistory } from 'src/app/interfaces/LoanHistory';
import { StopLoanRequest } from 'src/app/http/requests/StopLoanRequest';
import { LoanResponse } from 'src/app/interfaces/LoanResponse';
import { MatDialog } from '@angular/material';
import { PaypallDialogComponent } from '../paypall-dialog/paypall-dialog.component';
@Component({
  selector: 'app-start-loan',
  templateUrl: './start-loan.component.html',
  styleUrls: ['./start-loan.component.scss']
})
export class StartLoanComponent implements OnInit {
  unlock = true;
  return = true;
  stationId = 0;
  showstart = false;
  showreturn = false;
  powerbank: PowerbankDto = new PowerbankDto();
  userId: number;
  StopLoanData: LoanResponse;
  // Timer
  counter0 = 0;
	timer0Id: string;
	timer0Name = '1 sec';
  timer0button = 'Subscribe';
  totalSecondsLent = 0;
  totalMinutsLent = 0;
  totalPrice = 0;
  pricePer15Min = 0.25;
  constructor(
    private userSvc: UserService,
    private csSvc: ChargingstationService,
    private st: SimpleTimer,
    private csHttpSvc: CharginstationsHttpService,
    private router: Router,
    private userHttpSvc: UserHttpService,
    private userService: UserService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.unlock = true;
    this.userSvc.currentUserId.subscribe(data => {
      this.userId = data;
      this.userHttpSvc.GetLoanHistory(this.userId).subscribe((l: LoanHistory[] ) => {
        if (l.find(d => d.isOngoing === true)) {
          this.return = true;
        } else {
          this.return = false;
        }
        this.csSvc.currentStationId.subscribe(data => {
          this.stationId = data;
        });
        console.log(this.stationId);
        if ( this.stationId !== 0) {
          if ( this.return) {
            this.csHttpSvc.putReturnRequest(this.stationId, true).subscribe();
            const poll = this.csHttpSvc.pollReturnOk(this.stationId);
            const o = interval(1000)
            .pipe(
              startWith(0),
              switchMap(() => poll)
            ).subscribe(res => {
              console.log(res);
              if (res) {
                this.showreturn = true;
                this.showstart = false;
                this.stopLoan();
                o.unsubscribe();
              }
            }); // start teruggeven powerbank
          } else if (!this.return) {
            // start ontleen powerbank
            this.csHttpSvc.putLoanRequest(this.stationId, true).subscribe();
            const poll = this.csHttpSvc.pollAvailablePowerbank(this.stationId);
            const o = interval(1000)
            .pipe(
              startWith(0),
              switchMap(() => poll)
            ).subscribe(res => {
              console.log(res);
              if ( res.id !== 0) {
                this.powerbank = res;
                this.showstart = true;
                this.showreturn = false;
                o.unsubscribe();
              }
            });
          }
          this.counter0 = 0;
        } else {
          this.router.navigate(['/main/scanner']);
        }
      });
    });
  }
  startLoan() {
    this.unlock = false;
    const req: StartLoanRequest = { userId: this.userId, powerBankId: this.powerbank.id, stationId: this.stationId  };
    this.userHttpSvc.StartLoan(req).subscribe(data => {
      console.log(data);
      this.csHttpSvc.putOpenStation(this.stationId, true).subscribe();
    }
    );
    this.userHttpSvc.GetLoanHistory(this.userId).subscribe((data: LoanHistory[] ) => {
      this.userService.setLoanHistory(data);
    });
  }
  stopLoan() {
    const req: StopLoanRequest = { userId: this.userId, level: 20, loanStation: this.stationId };
    this.userHttpSvc.StopLoan(req).subscribe((data: LoanResponse) => {
       this.StopLoanData = new LoanResponse();
       this.StopLoanData = data;
       console.log(data);
    });
    this.userHttpSvc.GetLoanHistory(this.userId).subscribe((data: LoanHistory[] ) => {
      this.userService.setLoanHistory(data);
    });

  }
  pay() {
    this.router.navigate(['/main/maps']);
    this.csHttpSvc.putReturnRequest(this.stationId, false).subscribe();
    this.ResetStation();
    const dialogRef = this.dialog.open(PaypallDialogComponent, {
      width: '500px',
      data: {price: this.StopLoanData.price}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.ResetStation();
      this.router.navigate(['/main/maps']);
    });
  }
  close() {
    this.csHttpSvc.putOpenStation(this.stationId, false).subscribe();
    setTimeout(() => this.ResetStation() , 4000);
    this.router.navigate(['/main/maps']);
  }

  private ResetStation() {
    console.log('reset');
    this.csHttpSvc.putLoanRequest(this.stationId, false).subscribe();
    this.csHttpSvc.putReturnRequest(this.stationId, false).subscribe();
    this.csHttpSvc.resetReturnOK(this.stationId).subscribe();
  }
}
