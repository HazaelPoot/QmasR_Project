import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Proyecto, ProyectsResponse } from '../models/proyect.model';

@Injectable({
  providedIn: 'root'
})
export class ProyectosService {

  constructor(
    private http: HttpClient
  ) { }

  getAllProyects() {
    return this.http.get<ProyectsResponse>('https://localhost:7019/Proyecto/lista');
  }
}

