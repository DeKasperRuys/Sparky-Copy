import { Component, OnInit, OnDestroy } from '@angular/core';
import { ChargingStation } from '../../../interfaces/ChargingStations';
import { ChargingstationService } from '../../../services/chargingstation.service';
import { Observer } from 'firebase';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Auth2Service } from '../../../auth/auth2.service';
import { StationDetailDialogComponent } from './dialogs/station-detail-dialog/station-detail-dialog.component';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.scss']
})
export class MapsComponent implements OnInit, OnDestroy {
  constructor(
    private csService: ChargingstationService,
    private userService: UserService,
    private dialog: MatDialog,
    private Auth: Auth2Service
  ) {
    // Laoding moet worden disabled als we op de pagina komen
    this.Auth.loading = false;

    // Toont huidige locatie

    if (navigator) {
      navigator.geolocation.getCurrentPosition(pos => {
        this.lng = +pos.coords.longitude;
        this.lat = +pos.coords.latitude;
      });
    }
  }
  // pinSparky: string = "../../../../assets/images/MapsMarker.png";
  lat: any;
  lng: any;
  stations: ChargingStation[];
  csObserver: any;
  // Map styling -> verstopt points-of-interest
  public mapStyles = [
    {
      featureType: 'poi',
      elementType: 'labels',
      stylers: [
        {
          visibility: 'off'
        }
      ]
    }
  ];
  // pin styling -> gebruikt custom pin en resized
  public pinSparky = {
    url: '../../../../assets/images/MapsMarker.png',
    scaledSize: {
      width: 50,
      height: 50
    }
  };
  // Map styling -> verstopt points-of-interest
  isReturn: boolean;
  getStations() {
    this.csObserver = this.csService.ChargingStations.subscribe(stations => {
      if (stations) {
        this.stations = stations;
        this.stations.forEach(s => {
          s.mlat = s.lat;
          s.mlon = s.lon;
        });
      }
    });
  }
  ngOnInit() {
    this.getStations();
    this.userService.updateLoanHistory();
    this.userService.currentLoanHistory.subscribe(history => {
      if (history.find(d => d.isOngoing === true)) {
        this.isReturn = true;
      } else {
        this.isReturn = false;
      }
    });
    this.csService.updateCharginStations();
  }
  ngOnDestroy() {
    this.csObserver.unsubscribe();
  }
  // On Marker CLick open dialog
  openDialog(station: ChargingStation) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = false;
    dialogConfig.autoFocus = false;
    dialogConfig.data = station;
    const dialogRef = this.dialog.open(StationDetailDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(x => {
      this.csService.updateCharginStations();
    });
  }
}
