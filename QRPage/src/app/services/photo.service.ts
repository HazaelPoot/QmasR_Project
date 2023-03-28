import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs';
import { PhotoResponse } from '../models/photo.model';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(
    private http: HttpClient
  ) { }

  getPhotosById(idProj: number): Observable<PhotoResponse>{
    return this.http.get<PhotoResponse>(`https://localhost:7019/Foto/ListByProject?idProj=${idProj}`)
  }

}
