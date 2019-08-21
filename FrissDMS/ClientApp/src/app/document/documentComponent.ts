import { Component, Inject, OnInit, NgModule } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Router } from '@angular/router';
import { DocumentService } from '../../../Services/DocumentService';


@Component({
  selector: 'app-document',
  templateUrl: './document.component.html'
})

@NgModule({
  declarations: [DocumentService],
  exports: [DocumentService]
})

export class DocumentComponent implements OnInit {
  docServiceObject: DocumentService;
  progress: number;
  message: string;
  docs: any[];
  downloadUrl: string;
  baseUrl: string;
  http: HttpClient;
  loggerName: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
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

  uploadDocument(files): void {
    this.docServiceObject.uploadDocument(files).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.message = "Uploading document is in progress...";
        this.progress = Math.round(100 * event.loaded / event.total);
      } else if (event.type === HttpEventType.Response) {
        this.message = event.body.message;
        console.log(event);
        this.docs.push({ id: event.body.id, name: event.body.name });
      }
    }, error => console.error(error));
  }

  downloadDocument(id: number): void {
    console.log("inside downloadDocument component ... " + id);
    this.docServiceObject.downloadDocument(id).subscribe(result => {
      console.log("result = " + result);
      window.open(this.baseUrl + "Uploads/" + result.docName, "_self");
    }, error => console.error(error));
  }

  deleteDocument(id: number): void {
    console.log("delete " + id);
    const confirmDelete = confirm("Are you sure?");
    if (confirmDelete) {
      this.docServiceObject.deleteDocument(id).subscribe(() => {
        this.docs = this.docs.filter(u => u.id !== id);
      }, error => console.error(error));
    }
  }
}
