import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {
  authenticated: boolean = false;
  private subscription: Subscription;
  userName: string = '';

  constructor(private service: AuthenticationService) {

  }

     ngOnInit() {
      this.subscription = this.service.authenticationChallenge$.subscribe(res => {
          this.authenticated = res;
          this.userName = this.service.UserData.email;
      });

      if (window.location.hash) {
          this.service.AuthorizedCallback();
      }

      console.log('identity component, checking authorized' + this.service.IsAuthorized);
      this.authenticated = this.service.IsAuthorized;

      if (this.authenticated) {
          if (this.service.UserData)
              this.userName = this.service.UserData.email;
      }
  }

  logoutClicked(event: any) {
      event.preventDefault();
      console.log('Logout clicked');
      this.logout();
  }

  login() {
      this.service.Authorize();
  }

  logout() {
      this.service.Logoff();
  }

}
