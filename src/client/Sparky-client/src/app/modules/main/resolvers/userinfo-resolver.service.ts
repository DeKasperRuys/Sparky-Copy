import { Injectable } from '@angular/core';

import { ActivatedRouteSnapshot, RouterStateSnapshot, Resolve } from '@angular/router';

import { UserService } from 'src/app/services/user.service';
import { UserHttpService } from 'src/app/http/user-http.service';
import { LoanHistory } from 'src/app/interfaces/LoanHistory';
import { UserInfo } from 'src/app/interfaces/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class UserResolver implements Resolve<any> {
  private loanHistory: Array<LoanHistory> = [];
  constructor(private usersvc: UserService,private userHttpSvc: UserHttpService) { }
  async resolve(route: ActivatedRouteSnapshot, rstate: RouterStateSnapshot){
    const user: UserInfo = JSON.parse(localStorage.getItem("user"));
    this.usersvc.setUserInfo(user);
    let userId = 0;
    this.userHttpSvc.GetUser(user.uid).subscribe((data: UserInfo) => {
      this.usersvc.setUserId(data.userId);
      userId = data.userId;
      console.log(userId);
      this.userHttpSvc.GetLoanHistory(userId).subscribe((data: Array<LoanHistory>) => {
        this.loanHistory = data;
        console.log(this.loanHistory);
        this.usersvc.setLoanHistory(this.loanHistory);
      })
    });
    

  }
}