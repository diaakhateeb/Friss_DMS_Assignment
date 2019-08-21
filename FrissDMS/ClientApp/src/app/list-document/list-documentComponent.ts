import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DocumentService } from '../../../Services/DocumentService';


@Component({
  selector: 'app-list-document',
  templateUrl: './list-document.component.html'
})

export class ListDocumentComponent implements OnInit {
  docServiceObject: DocumentService;
  progress: number;
  message: string;
  docs: any[];
  downloadUrl: string;
  baseUrl: string;
  http: HttpClient;
  loggerName: string;

  constructor(http: HttpClient, private router: Router,
    @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
    this.docServiceObject = new DocumentService(http, baseUrl);
  }

  ngOnInit(): void {
    this.loggerName = localStorage.getItem("fullName");
    this.docServiceObject.getAllDocuments().subscribe(result => {
      this.docs = result;
      console.log(this.docs);
      console.log("docs in init " + this.docs);
    }, error => console.error(error));
  }

  downloadDocument(id: number): void {
    console.log("inside downloadDocument component ... " + id);
    this.docServiceObject.downloadDocument(id).subscribe(result => {
      window.open(this.baseUrl + "Uploads/" + result.docName, "_self");
    }, error => console.error(error));
  }
}
