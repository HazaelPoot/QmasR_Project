import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ObservableInput } from 'rxjs';
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
    return this.http.get<ProyectsResponse>('https://localhost:7019/Proyecto/ListActivate');
  }

  // getProJectById(idProj: number): Observable<ProyectsResponse> {
  //   const url = `https://localhost:7019/Proyecto/GetById/${idProj}`; // construye la URL de la API para obtener el producto por ID
  //   return this.http.get<ProyectsResponse>(url); // hace una solicitud GET al servidor y devuelve el resultado como un observable
  // }

  getProJectById(idProj: number): Observable<Proyecto>{
    return this.http.get<Proyecto>(`https://localhost:7019/Proyecto/GetById/${idProj}`)
  }

  getPhotosById(idProj: number): Observable<Proyecto>{
    return this.http.get<Proyecto>(`https://localhost:7019/Foto/ListByProject?idProj=${idProj}`)
  }


  getAllCategories() {
    return this.http.get<CategorysResponse>('https://localhost:7019/Categoria/lista');
  }
}

