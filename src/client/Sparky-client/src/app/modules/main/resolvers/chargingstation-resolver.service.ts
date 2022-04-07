import { Injectable } from '@angular/core';
import { CharginstationsHttpService } from 'src/app/http/Chargingstations-http.service';
import { ChargingstationService } from 'src/app/services/chargingstation.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Resolve } from '@angular/router';
import { ChargingStation } from 'src/app/interfaces/ChargingStations';

@Injectable({
  providedIn: 'root'
})
export class ChargingstationResolver implements Resolve<any> {
  chargingstations: Array<ChargingStation> = [];

  constructor(private csHttp: CharginstationsHttpService, private csService: ChargingstationService ) { }
  async resolve(route: ActivatedRouteSnapshot, rstate: RouterStateSnapshot){
    this.csHttp.getStations().subscribe((data: any[]) => {
      this.chargingstations = data;
      this.csService.setChargingStations(this.chargingstations);
    });
  }
}
