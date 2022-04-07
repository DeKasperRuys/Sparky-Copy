import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { MapsComponent } from './maps/maps.component';
import { Routes, RouterModule } from '@angular/router';
import { NavigationComponent } from './navigation/navigation.component';
import { AgmCoreModule } from '@agm/core';
import { AuthGuard } from '../../auth/auth.guard';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { QrcodeComponent } from './qrcode/qrcode.component';
import { UserService } from 'src/app/services/user.service';
import { UserHttpService } from 'src/app/http/user-http.service';
import { UserResolver } from './resolvers/userinfo-resolver.service';
import { HistoryComponent } from './history/history.component';
import {MatCardModule} from '@angular/material/card';
import { StartLoanComponent } from './start-loan/start-loan.component';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatButtonModule} from '@angular/material/button';
import { MatDialogModule, MAT_DIALOG_DEFAULT_OPTIONS, MatToolbarModule, MatSidenavModule } from '@angular/material';
import { ChargingstationResolver } from './resolvers/chargingstation-resolver.service';
import { CharginstationsHttpService } from 'src/app/http/Chargingstations-http.service';
import { PaypallDialogComponent } from './paypall-dialog/paypall-dialog.component';
import { StationDetailDialogComponent } from './maps/dialogs/station-detail-dialog/station-detail-dialog.component';
import { TimePipe } from './pipes/TimePipe';
import { HowdoesitworkComponent } from './howdoesitwork/howdoesitwork.component';
const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    resolve: {
      stations: ChargingstationResolver,
      users: UserResolver
    },
    children: [
      {
        path: '',
        redirectTo: 'maps',
        pathMatch: 'full',
        canActivate: [AuthGuard]
      },
      {
        path: 'maps',
        component: MapsComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'scanner',
        component: QrcodeComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'startloan',
        component: StartLoanComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'history',
        component: HistoryComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'how-does-it-work',
        component: HowdoesitworkComponent,
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  declarations: [
    MainComponent,
    MapsComponent,
    NavigationComponent,
    QrcodeComponent,
    HistoryComponent,
    StartLoanComponent,
    PaypallDialogComponent,
    StationDetailDialogComponent,
    TimePipe,
    HowdoesitworkComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    ZXingScannerModule,
    RouterModule.forChild(routes),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyA68WHQEhVwQ8f9bMBWvBemdsWUYdnuXJQ'
    }),
    MatCardModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule
  ],
  providers: [ChargingstationResolver,
    UserResolver, UserService,
    UserHttpService, CharginstationsHttpService, {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}}]
  , entryComponents: [StationDetailDialogComponent, PaypallDialogComponent]

})
export class MainModule {}
