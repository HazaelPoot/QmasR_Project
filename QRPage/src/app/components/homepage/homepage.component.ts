import { Component, OnInit } from '@angular/core';

import * as Aos from 'aos';
import { Proyecto } from 'src/app/models/proyect.model';
import { ProyectosService } from 'src/app/services/proyectos.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']

})

export class HomepageComponent implements OnInit {

  proyectos: Proyecto[] = [];

  constructor(
    private proyectoService: ProyectosService
  ) {}

  ngOnInit(): void {
    this.proyectoService.getAllProyects().subscribe((response) => {
      console.log(response.data);
      this.proyectos = response.data;
      //this.proyectos = response.data.filter(proyecto => proyecto.idCategoria == 1);
      console.log(this.proyectos);
     },
    )
   }
}


