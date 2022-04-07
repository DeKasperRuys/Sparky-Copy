import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../auth/authentication.service";
import { Auth2Service } from "src/app/auth/auth2.service";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"]
})
export class LoginComponent implements OnInit {
  constructor(public authService: Auth2Service) {}

  ngOnInit() {}
}
