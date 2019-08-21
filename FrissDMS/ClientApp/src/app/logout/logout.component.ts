import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'Services/AuthService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Logout',
  templateUrl: './Logout.component.html'
})

export class LogoutComponent implements OnInit {
  ngOnInit(): void {
    this.authService.logoutUser();
    localStorage.clear();
    this.router.navigate(["login"]);
  }

  authService: AuthService;
  router: Router;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, router: Router) {
    this.authService = new AuthService(http, baseUrl);
    this.router = router;
  }
}
