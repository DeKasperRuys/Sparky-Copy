import { Injectable, NgZone } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { Router } from '@angular/router';
import { UserInfo } from '../interfaces/UserInfo';
import { UserService } from '../services/user.service';
import { UserHttpService } from '../http/user-http.service';
import { auth } from 'firebase';
@Injectable({
  providedIn: 'root'
})
export class Auth2Service {
  user: UserInfo;
  jwt: string;
  loading = false;
  constructor(
    private afAuth: AngularFireAuth,
    private router: Router,
    private userService: UserService,
    private userHttpsvc: UserHttpService,
    private ngZone: NgZone
  ) {
    this.user = { displayName: '', email: '', uid: '', userId: 0 };
  }
  // DEPRICATED
  // LORENZO ==> heb auth service herschreven zodat die samen gaat met eigen database.
  // moet email verificatie nog terug toevoegen en reset password ook.
  AnalogSingIn(email, pw) {
    return this.afAuth.auth
      .signInWithEmailAndPassword(email, pw)
      .then(result => {
        if (result.user) {
          this.user = this.Map(result.user);
          console.log(this.user);
          this.userService.setUserInfo(this.user);
          this.userHttpsvc
            .AddUserInfo(this.Map(result.user))
            .subscribe(result => {
              if (result) {
                this.ngZone.run(() => this.router.navigate(['/main/maps']));
              }
            });
        } else {
          localStorage.setItem('user', null);
        }
      });
  }
  AnalogSingUp(email, pw) {
    return this.afAuth.auth
      .createUserWithEmailAndPassword(email, pw)
      .then(result => {
        if (result.user) {
          this.userHttpsvc
            .AddUserInfo(this.Map(result.user))
            .subscribe(result => {
              if (result) {
                this.ngZone.run(() => this.router.navigate(['/intro/login']));
              }
            });
        }
      })
      .catch(error => {
        window.alert(error.message);
      });
  }
  async SingInWithGoogle() {
    this.loading = true;
    return await this.afAuth.auth
      .signInWithPopup(new auth.GoogleAuthProvider())
      .then(result => {
        if (result.user) {
          this.user = this.Map(result.user);
          console.log(this.user);
          this.userService.setUserInfo(this.user);
          this.userHttpsvc
            .AddUserInfo(this.Map(result.user))
            .subscribe(result => {
              if (result) {

                this.ngZone.run(() => this.router.navigate(['/main/maps']));

              }
            });
        }
      })
      .catch(error => {
        window.alert(error);
      });
  }
  SingOut() {
    return this.afAuth.auth.signOut().then(() => {
      localStorage.removeItem('user');
      this.ngZone.run(() => this.router.navigate(['/intro/login']));
    });
  }
  Map(user: firebase.User): UserInfo {
    const usr: UserInfo = { displayName: '', email: '', uid: '', userId: 0 };
    usr.displayName = user.displayName;
    usr.email = user.email;
    usr.uid = user.uid;
    return usr;
  }
}
