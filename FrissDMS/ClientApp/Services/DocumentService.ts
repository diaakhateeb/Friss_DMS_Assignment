import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class DocumentService {
  readonly httpClient: HttpClient;
  private readonly baseUrl: string;
  progress: number;
  message: string;
  //headers = new Headers({
  //  'Cache-Control': 'no-cache',
  //  'Pragma': 'no-cache'
  //});

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  getAllDocuments(): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "api/Document/GetDocuments");
  }

  uploadDocument(files): Observable<any> {
    if (files.length === 0)
      return undefined;

    const data = new FormData();

    for (let file of files) {
      data.append(file.name, file);
    }
    console.log("data = " + data);
    const uploadReq = new HttpRequest('post', this.baseUrl + "api/Document/Upload", data, { reportProgress: true, });

    return this.httpClient.request(uploadReq);
  }

  downloadDocument(id: number): Observable<any> {
    const data = { id: String(id) };
    console.log("inside downloadDocument service " + id);
    console.log(data);

    return this.httpClient.get<any>(this.baseUrl + "api/Document/Download", { params: data });
  }

  deleteDocument(id: number): Observable<any> {
    const data = { id: String(id) };

    return this.httpClient.delete<any>(this.baseUrl + "api/Document/DeleteDocument", { params: data });
  }
}
