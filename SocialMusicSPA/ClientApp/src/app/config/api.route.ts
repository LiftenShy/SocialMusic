export class API {
  public Identity = new Identity();
}


 class Identity {
  public Account = new Account();
 }

 class Account {
  public Login = 'http://localhost:5005/api/Account/Login';
 }
