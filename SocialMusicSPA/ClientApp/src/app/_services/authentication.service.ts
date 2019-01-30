import { Injectable }                   from '@angular/core';
import { Observable, Subject }          from 'rxjs';
import { map }                          from 'rxjs/operators';
import { Router }                       from '@angular/router';
import { ActivatedRoute }               from '@angular/router';
import { StorageService }               from './storage.service';
import { HttpClient, HttpResponse, HttpHeaders, HttpParams } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private actionUrl: string;
  private headers: HttpHeaders;
  private storage: StorageService;
  private authenticationSource = new Subject<boolean>();
  authenticationChallenge$ = this.authenticationSource.asObservable();
  private authorityUrl = 'http://localhost:5005';
  private settingsLoadedSource = new Subject();
  settingsLoaded$ = this.settingsLoadedSource.asObservable();

  constructor(private _http: HttpClient,
    private _router: Router,
    private route: ActivatedRoute,
    private _storageService: StorageService) {

    this.headers = new HttpHeaders()
        .set('Content-type', 'application/json')
        .set('Accept', 'application/json');
    this.storage = _storageService;

    this.storage.store('identityUrl', this.authorityUrl);

    if (this.storage.retrieve('IsAuthorized') !== '') {
      this.IsAuthorized = this.storage.retrieve('IsAuthorized');
      this.authenticationSource.next(true);
      this.UserData = this.storage.retrieve('userData');
    }
  }

  public IsAuthorized: boolean;
  public UserData: any;

  public GetToken(): any {
    return this.storage.retrieve('authorizationData');
  }

  public ResetAuthorizationData() {
    this.storage.store('_authorizationData', '');
    this.storage.store('authorizationDataIdToken', '');

    this.IsAuthorized = false;
    this.storage.store('IsAuthorized', false);
  }

  public SetAuthrizationData(token: any, id_token: any) {
    if (this.storage.retrieve('authorizationData') !== '') {
        this.storage.store('authorizationData', '');
    }

    this.storage.store('authorizationData', token);
    this.storage.store('authorizationDataIdToken', id_token);
    this.IsAuthorized = true;
    this.storage.store('IsAuthorized', true);

    this.getUserData();
  }

  public Authorize() {
    this.ResetAuthorizationData();

    let authorizationUrl = `${this.authorityUrl}/connect/authorize`;
    let client_id = 'js';
    let redirect_uri = `${location.origin}/`
    let response_type = 'id_token token';
    let scope = 'openid profile storage';
    let nonce = `N${Math.random()}${Date.now()}`;
    let state = `${Date.now()}${Math.random()}`;

    this.storage.store('authStateControl', state);
    this.storage.store('authNonce', nonce);

    let url = `${authorizationUrl}?response_type=${response_type}&client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&nonce=${nonce}&state=${state}`;

    window.location.href = url;
  }

  public AuthorizedCallback() {
    this.ResetAuthorizationData();

    let hash = window.location.hash.substr(1);

    let result: any = hash.split('&').reduce((result:any, item: string) => {
       let parts = item.split('=');
       result[parts[0]] = parts[1];
       return result;
    }, {})

    console.log(result);

    let token = '';
    let id_token = '';
    let authResponseIsValid = false;

    if(!result.error) {
      if (result.state !== this.storage.retrieve('authStateControl')) {
        console.log('AuthorizedCallback incorrect state')
      } else {

        token = result.access_token;
        id_token = result.id_token;

        let dataIdToken: any = this.getDataFromToken(id_token);
        console.log(dataIdToken);

        if(dataIdToken.nonce !== this.storage.retrieve('authNonce')) {
          console.log('AuthorizedCallback incorrect nonce');
        } else {
          this.storage.store('authNonce', '');
          this.storage.store('authStateControl', '');

          authResponseIsValid = true;
          console.log('AuthorizedCallback state and nonce validated, returning access token');
        }
      }
    }

    if(authResponseIsValid) {
      this.SetAuthrizationData(token, id_token);
    }
  }

  public Logoff() {
    let authorizationUrl = `${this.authorityUrl}/connection/endsession`;
    let id_token_hint = this.storage.retrieve('authorizationDataIdToken');
    let post_logout_redirect_url = `${location.origin}/`;

    let url = `${authorizationUrl}?id_token_hint=${encodeURI(post_logout_redirect_url)}`

    this.ResetAuthorizationData();

    this.authenticationSource.next(false);
    window.location.href = url;
  }

  public HandleError(error: any) {
    console.log(error);
    if(error.status === 403) {
      this._router.navigate(['/Forbidden']);
    }
    else if (error.status === 401) {
      this._router.navigate(['/Unauthorized']);
    }
  }

  private urlBase64Decode(str: string) {
    let output = str.replace('-', '+').replace('_', '/');

    switch (output.length % 4) {
      case 0:
        break;
      case 2:
        output += '=='
        break;
      case 3:
        output += '=';
        break;
      default:
        throw 'Illegal base64url string!';
    }

    return window.atob(output);
  }

  private getDataFromToken(token: any) {
    let data = {};
    if (typeof token !== 'undefined') {
       let encoded = token.split('.')[1];
       data = JSON.parse(this.urlBase64Decode(encoded));
    }

    return data;
  }

  private getUserData = () => {
    this.setHeaders();
    if (this.authorityUrl === '') {
      this.authorityUrl = this.storage.retrieve('IdentityUrl');
    }

    this._http.get(this.authorityUrl + '/connect/userinfo', {
      headers: this.headers
    }).subscribe(
        data => {
        this.UserData = data;
        this.storage.store('userData', data);

        this.authenticationSource.next(true);
        window.location.href = location.origin;
      },
      error => this.HandleError(error),
      () => {
        console.log(this.UserData);
      });
  }

  private setHeaders() {
    this.headers = new HttpHeaders().set('Content-Type', 'application/json').set('Accept', 'application/json');

    let token = this.GetToken();

    if(token !== '') {
      this.headers = this.headers.set('Authorization', `Bearer ${token}`);
    }
  }
}
