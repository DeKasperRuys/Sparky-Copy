import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../auth/authentication.service";
import { Auth2Service } from 'src/app/auth/auth2.service';

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"]
})
export class RegisterComponent implements OnInit {
  constructor(public authService: Auth2Service) {}

  ngOnInit() {}
}
