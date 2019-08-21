import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from "../Services/UserService";

export class AuthGuardService implements CanActivate {
  readonly httpClient: HttpClient;
  private readonly baseUrl: string;
  userService: UserService;

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string, private router: Router) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.userService = new UserService(http, baseUrl);
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    if (localStorage.getItem('username') != null) {
      const roles = next.data["roles"] as Array<string>;
      if (roles) {
        const match = this.userService.roleMatch(roles);
        if (match) return true;
        else {
          this.router.navigate(['/access-denied']);
          return false;
        }
      }
      else
        return true;
    }
    this.router.navigate(['/login']);
    return false;
  }
}
