import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LoginComponent } from "../intro/login/login.component";
import { Routes, RouterModule } from "@angular/router";
import { AuthService } from "../../auth/authentication.service";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RegisterComponent } from "../intro/register/register.component";
import { SecureInnerPagesGuard } from "../../auth/secure-inner-pages-guard";
import { AuthGuard } from "../../auth/auth.guard";
import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
import { VerifyEmailComponent } from "./verify-email/verify-email.component";
import { Auth2Service } from "src/app/auth/auth2.service";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

const routes: Routes = [
  {
    path: "",
    redirectTo: "login",
    pathMatch: "full"
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "verify-email",
    component: VerifyEmailComponent,
    canActivate: [SecureInnerPagesGuard]
  },
  {
    path: "forgot-password",
    component: ForgotPasswordComponent,
    canActivate: [SecureInnerPagesGuard]
  },
  {
    path: "register",
    component: RegisterComponent,
    canActivate: [SecureInnerPagesGuard]
  }
];

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    ForgotPasswordComponent,
    VerifyEmailComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule
  ],
  providers: [Auth2Service, AuthGuard]
})
export class IntroModule {}
