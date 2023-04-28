import { ProyectosService } from 'src/app/services/proyectos.service';
import { PhotoService } from 'src/app/services/photo.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Proyecto } from 'src/app/models/proyect.model';
import { Photo } from 'src/app/models/photo.model';
import { Component } from '@angular/core';

@Component({
  selector: 'app-proyectodetail',
  templateUrl: './proyectodetail.component.html',
  styleUrls: ['./proyectodetail.component.scss']
})
export class ProyectodetailComponent {

  idProj!: number;
  proyecto!: Proyecto;
  photos: Photo[] = []

  constructor(
    private activatedRoute: ActivatedRoute,
    private photoService: PhotoService,
    private proyectoService: ProyectosService,
    )
    {
      this.idProj = Number(this.activatedRoute.snapshot.paramMap.get('idProj'));
    }

  ngOnInit() : void {
    this.obtenerProyecto();
    this.obtenerPhotos();
  }

  obtenerProyecto(){
    this.proyectoService.getProJectById(this.idProj).subscribe(data => {
      this.proyecto = data;
    })
  }

  obtenerPhotos(){
    this.photoService.getPhotosById(this.idProj).subscribe(data => {
      this.photos = data.data;
    })
  }
}
