import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { SwiperModule } from 'swiper/angular';
import { HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AboutComponent } from './components/about/about.component';
import { CarouselComponent } from './components/carousel/carousel.component';

import { ContactComponent } from './components/contact/contact.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UsuariosComponent } from './components/usuarios/usuarios.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SidebarService } from './services/sidebar.service';
import { ProyectoComponent } from './components/proyecto/proyecto.component';
import { ImgComponent } from './components/img/img.component';
import { ToArrayPipe } from './components/pipes/to-array.pipe';


@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    CarouselComponent,
    AboutComponent,
    ContactComponent,
    LoginComponent,
    DashboardComponent,
    UsuariosComponent,
    SidebarComponent,
    ProyectoComponent,
    ImgComponent,
    ToArrayPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    SwiperModule,
    HttpClientModule
  ],
  providers: [SidebarService,],
  bootstrap: [AppComponent, ]
})
export class AppModule { }
