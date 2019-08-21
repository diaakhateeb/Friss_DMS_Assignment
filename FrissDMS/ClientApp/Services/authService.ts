import { Inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class AuthService {
  readonly httpClient: HttpClient;
  private readonly baseUrl: string;
  //headers = new Headers({
  //  'Cache-Control': 'no-cache',
  //  'Pragma': 'no-cache'
  //});

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  loginUser(username: string, password: string): Observable<any> {
    const data = { username: username, password: password };

    //return this.httpClient.get<any>(this.baseUrl + "api/User/GetUserById", { params: data });
    return this.httpClient.get<any>(this.baseUrl + "api/Authentication/Login", { params: data });
  }

  registerUser(name: string, username: string, password: string, email: string, role: string): Observable<any> {
    const data = { name: name, email: email, username: username, password: password, role: role };

    //return this.httpClient.get<any>(this.baseUrl + "api/User/GetUserById", { params: data });
    return this.httpClient.post<any>(this.baseUrl + "api/Authentication/Register", { params: data });
  }

  logoutUser(): void {
    this.httpClient.get<any>(this.baseUrl + "api/Authentication/Logout");
  }
}
