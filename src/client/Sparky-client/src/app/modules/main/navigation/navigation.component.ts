import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../auth/authentication.service";

@Component({
  selector: "app-navigation",
  templateUrl: "./navigation.component.html",
  styleUrls: ["./navigation.component.scss"]
})
export class NavigationComponent implements OnInit {
  constructor(public auth: AuthService) {}

  ngOnInit() {}
}
