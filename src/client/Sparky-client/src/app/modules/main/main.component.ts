import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { ChargingstationService } from 'src/app/services/chargingstation.service';
import { ChargingStation } from 'src/app/interfaces/ChargingStations';
import { UserService } from 'src/app/services/user.service';
import { UserInfo } from 'src/app/interfaces/UserInfo';
import { Auth2Service } from 'src/app/auth/auth2.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit, OnDestroy {
  uid = '';
  stations: ChargingStation[];
  user: UserInfo;
  observer1: any;
  observer2: any;
  constructor(
    public auth: Auth2Service,
    private csService: ChargingstationService,
    private userService: UserService,
    private router: Router,
    private ngZone: NgZone) {}
  ngOnInit() {
    this.observer1 = this.csService.ChargingStations.subscribe(stations => {
      this.stations = stations;
    });
    this.observer2 = this.userService.currentUser.subscribe(user => {
      this.user = user;
    });
  }
  // async getJwt(){
  //   this.jwt = await this.authsvc.getJwt();
  // }
  navigate(path) {
    this.ngZone.run(() => this.router.navigate([path]));
  }
  updateLoanHistory() {
    this.userService.updateLoanHistory();
  }
  ngOnDestroy() {
    this.observer1.unsubscribe();
    this.observer2.unsubscribe();
  }
}
