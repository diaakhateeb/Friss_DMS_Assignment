import { Inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class UserService {
  readonly httpClient: HttpClient;
  private readonly baseUrl: string;

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  getAllUsers(): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "api/User/GetUsers");
  }

  getUser(id: number): Observable<any> {
    const data = { id: String(id) };

    return this.httpClient.get<any>(this.baseUrl + "api/User/GetUserById", { params: data });
  }

  addUser(name: string, email: string, username: string, password: string): Observable<any> {
    const data = { name: name, email: email, username: username, password: password };

    return this.httpClient.post<any>(this.baseUrl + "api/User/AddUser", { params: data });
  }

  editUser(id: number, name: string, username: string, email: string, password: string, role: string): Observable<any> {
    const data = { id: id, name: name, username: username, email: email, password: password, role: role };
    console.log(data);

    return this.httpClient.put<any>(this.baseUrl + "api/User/EditUser", data);
  }

  deleteUser(id: number): Observable<any> {
    const data = { id: String(id) };

    return this.httpClient.delete<any>(this.baseUrl + "api/User/DeleteUser", { params: data });
  }

  roleMatch(allowedRoles: string[]): boolean {
    var isMatch = false;
    console.log("userRoles = " + JSON.parse(JSON.stringify(localStorage.getItem('userRoles'))));
    var payLoad = JSON.parse(JSON.parse(JSON.stringify(localStorage.getItem('userRoles'))));
    console.log("payLoad " + payLoad);
    allowedRoles.forEach(element => {
      if (payLoad === element) {
        isMatch = true;
      }
    });
    return isMatch;
  }
}
