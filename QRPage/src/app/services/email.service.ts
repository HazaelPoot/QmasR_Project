import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs';
import { Email } from '../models/email.model';

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  constructor(
    private http: HttpClient
  ) { }

  // getPhotosById(idProj: number): Observable<PhotoResponse>{
  //   return this.http.get<PhotoResponse>(`https://localhost:7019/Foto/ListByProject?idProj=${idProj}`)
  // }

  sendEmail(email: Email): Observable<Email>{
    return this.http.post<Email>("https://localhost:7019/Api/Email", email)
  }

}
