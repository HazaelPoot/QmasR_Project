export interface Proyecto {
  idProj: number;
  titulo: string;
  descripcion: string;
  ubicacion: string;
  presupuesto: number;
  idCategoria: number;
  categoriaName: string;
  status: number;
  urlImagen: string;
}

export interface ProyectsResponse {
  data: Proyecto[];
}