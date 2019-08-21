import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.css']
})

export class AccessDeniedComponent implements OnInit {
  ngOnInit(): void {
    this.loggerName = localStorage.getItem("fullName");
  }

  baseUrl: string;
  http: HttpClient;
  loggerName: string;

  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }
}
