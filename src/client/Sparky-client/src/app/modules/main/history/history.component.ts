import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { LoanHistory } from 'src/app/interfaces/LoanHistory';
import { Observer, Subscription } from 'rxjs';
import { prepareEventListenerParameters } from '@angular/compiler/src/render3/view/template';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit, OnDestroy {

  constructor(private userSvc: UserService) { }
  loanHistory: Array<LoanHistory> = [];
  obs1: Subscription;
  totalHours;
  totalPrice;
  ngOnInit() {
    this.obs1  = this.userSvc.currentLoanHistory.subscribe((data: LoanHistory[]) => {
      data.forEach(loan => {
        loan.date = loan.date.substr(0, loan.date.indexOf('T'));
        this.loanHistory = data;
        this.totalHours = this.loanHistory.reduce((prev, val) => prev + val.duration, 0);
        this.totalPrice = this.loanHistory.reduce((prev, val) => prev + val.price, 0);
      });
      this.loanHistory.reverse();
      this.loanHistory.filter(x => x.isOngoing === false);
    });
  }
  ngOnDestroy(): void {
    this.obs1.unsubscribe();
  }
}
