import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Proyecto } from 'src/app/models/proyect.model';

@Component({
  selector: 'app-proyecto',
  templateUrl: './proyecto.component.html',
  styleUrls: ['./proyecto.component.scss']
})
export class ProyectoComponent {
  @Input() proyect: Proyecto = {
    idProj: 0,
    titulo: '',
    ubicacion: '',
    presupuesto: 0,
    status: 0,
    urlImagen: '',
    descripcion: '',
    idCategoria: 0,
    categoriaName: ''
  };
}
