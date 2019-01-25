import { Component } from '@angular/core';
import { AuthenticationService } from './_services/authentication.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  Authenticated: boolean = false;
  subscription: Subscription;

  constructor(private securityService: AuthenticationService) {
    this.Authenticated = this.securityService.IsAuthorized;
  }

  ngOnInit() {
    console.log('app on init');
    this.subscription = this.securityService.authenticationChallenge$.subscribe(res => this.Authenticated = res);
  }
}
