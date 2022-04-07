import { Injectable } from '@angular/core';
import { UserInfo } from '../interfaces/UserInfo';
import { BehaviorSubject } from 'rxjs';
import { LoanHistory } from '../interfaces/LoanHistory';
import { UserHttpService } from '../http/user-http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: UserInfo;
  loanHistory: Array<LoanHistory> = [];
  userId = 0;
  constructor(private userHttpService: UserHttpService) { }
  private sourceUser = new BehaviorSubject<UserInfo>(this.user);
  private sourceLoanHistory = new BehaviorSubject<Array<LoanHistory>>(this.loanHistory);
  private sourceUserId = new BehaviorSubject<number>(this.userId);
  currentUser = this.sourceUser.asObservable();
  currentLoanHistory = this.sourceLoanHistory.asObservable();
  currentUserId = this.sourceUserId.asObservable();
  setUserInfo(data: UserInfo) {
    this.sourceUser.next(data);
    localStorage.setItem('user', JSON.stringify(data));
  }
  setLoanHistory(history: Array<LoanHistory>) {
    this.sourceLoanHistory.next(history);
  }
  setUserId(userId: number) {
    this.sourceUserId.next(userId);
  }
  updateLoanHistory() {
    this.currentUserId.subscribe(id => {
      this.userHttpService.GetLoanHistory(id).subscribe((history: LoanHistory[]) => {
        this.sourceLoanHistory.next(history);
      });
    });
  }
}
