import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { CategorysResponse } from '../models/category.model';
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

  //HAZAEL ADD
  getAllCategories() {
    return this.http.get<CategorysResponse>('https://localhost:7019/Categoria/lista');
  }
}

