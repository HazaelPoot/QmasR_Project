import { Proyecto, ProyectsResponse } from '../models/proyect.model';
import { CategorysResponse } from '../models/category.model'
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProyectosService {

  url = 'https://localhost:7019/Proyecto'

  constructor(
    private http: HttpClient
  ) { }

  getProyects() {
    return this.http.get<ProyectsResponse>(`${this.url}/ListActivate`);
  }

  getProJectById(idProj: number): Observable<Proyecto>{
    return this.http.get<Proyecto>(`${this.url}/GetById/${idProj}`)
  }
}