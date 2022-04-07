import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AngularFirestore, AngularFirestoreModule } from '@angular/fire/firestore';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularFireModule } from '@angular/fire';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth/authentication.service';
import { AuthGuard } from './auth/auth.guard';
import { AgmCoreModule } from '@agm/core';
import { HttpClientModule } from '@angular/common/http';
import { ServiceWorkerModule } from '@angular/service-worker';
import { SimpleTimer } from 'ng2-simple-timer';
import {MatFormFieldModule, MatDialogModule} from '@angular/material';
import { from } from 'rxjs';



@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularFireModule.initializeApp(environment.firebaseConfig),
    AngularFireAuthModule,
    AngularFirestoreModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyC5oJrlsbjQLhEHcKw5d1oZH3_-gbnL3a4'
    }),
    HttpClientModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    MatDialogModule,
    MatFormFieldModule
  ],

  providers: [AuthService, AuthGuard, AngularFirestore, SimpleTimer],
  bootstrap: [AppComponent]
})
export class AppModule {}
