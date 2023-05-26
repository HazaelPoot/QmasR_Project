import { PhotoResponse } from '../models/photo.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  url = 'https://localhost:7019/Foto'

  constructor(
    private http: HttpClient
  ) { }

  getPhotosById(idProj: number): Observable<PhotoResponse>{
    return this.http.get<PhotoResponse>(`${this.url}/ListByProject?idProj=${idProj}`)
  }
}