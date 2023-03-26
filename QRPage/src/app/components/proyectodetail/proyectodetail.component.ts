import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { Photo } from 'src/app/models/photo.model';
import { Proyecto } from 'src/app/models/proyect.model';
import { PhotoService } from 'src/app/services/photo.service';
import { ProyectosService } from 'src/app/services/proyectos.service';

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
      // console.log(this.idProj);
    }

  ngOnInit() : void {
    this.obtenerProyecto();
     this.obtenerPhotos();
  }

  obtenerProyecto(){
    this.proyectoService.getProJectById(this.idProj).subscribe(data => {
      this.proyecto = data;
      // console.log(data)
    })
  }

  obtenerPhotos(){
    this.photoService.getPhotosById(this.idProj).subscribe(data => {
      this.photos = data.data;
      console.log(this.photos)
    })
  }
}
