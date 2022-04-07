import { Component, OnInit, NgZone } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-howdoesitwork',
  templateUrl: './howdoesitwork.component.html',
  styleUrls: ['./howdoesitwork.component.scss']
})
export class HowdoesitworkComponent implements OnInit {


  constructor(private ngZone: NgZone, private router: Router) { }

  ngOnInit() {
  }
  navigate(path) {
    this.ngZone.run(() => this.router.navigate([path]));
  }
}
