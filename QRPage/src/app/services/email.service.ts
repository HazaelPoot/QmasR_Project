import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs';
import { Email } from '../models/email.model';

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  url = 'https://localhost:7019/Api/Email'

  constructor(
    private http: HttpClient
  ) { }

  sendEmail(email: Email): Observable<Email>{
    return this.http.post<Email>(this.url, email)
  }
}