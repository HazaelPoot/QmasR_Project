export interface Photo{
  idImg: number;
  nombreFoto: string;
  urlImage: string;
  idProj: number;
  nombreProyecto: string;

}

export interface PhotoResponse {
  data: Photo[];
}