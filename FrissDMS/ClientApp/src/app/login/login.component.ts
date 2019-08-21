import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'Services/AuthService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Login',
  templateUrl: './Login.component.html'
})
export class LoginComponent implements OnInit {
  ngOnInit(): void {
    this.authService.logoutUser();
    localStorage.clear();
  }

  authService: AuthService;
  username: string;
  password: string;
  router: Router;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, router: Router) {
    this.authService = new AuthService(http, baseUrl);
    this.router = router;
  }

  loginUser(): void {
    this.authService.loginUser(this.username, this.password).subscribe(result => {
      localStorage.setItem("username", result.username);
      localStorage.setItem("fullName", result.fullName);
      localStorage.setItem("userRoles", JSON.stringify(result.role));

      if (localStorage.getItem("userRoles") === "Admin") {
        console.log("in admin");
        this.router.navigate(["document"]);
      }

      this.router.navigate(["list-document"]);
    },
      error => {
        this.router.navigate(["login"]);
      });
  }

  goRegistration(): void {
    this.router.navigate(["registration"]);
  }
}
