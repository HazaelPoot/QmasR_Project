import { ProyectosService } from 'src/app/services/proyectos.service';
import { Categoria } from 'src/app/models/category.model';
import { Proyecto } from 'src/app/models/proyect.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']

})

export class HomepageComponent implements OnInit {

  proyectos: Proyecto[] = [];
  claseSeleccionada:  Categoria[] = [];

  constructor(
    private proyectoService: ProyectosService
  ) {}

  ngOnInit(){
    this.getAllProyects();
  }

  getAllProyects(){
    this.proyectoService.getProyects().subscribe(
      (response) => {
      this.proyectos = response.data;
     })
  }
}