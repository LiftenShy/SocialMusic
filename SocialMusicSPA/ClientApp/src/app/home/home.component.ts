import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {
  authenticated: boolean = false;

  constructor(private service: AuthenticationService) {

    this.authenticated = this.service.IsAuthorized;
   }

  ngOnInit() {
  }

}
