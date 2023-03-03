namespace QRDashboard.Domain.Dtos
{
    public class DtoProyecto
    {
        public int IdProj { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Ubicacion { get; set; } = null!;
        public decimal Presupuesto { get; set; }
        public int? IdCategoria { get; set; }
        public string CategoriaName { get; set; } = null!;
        public int Status { get; set; } = 0;
        public string UrlImagen { get; set; } = null!;
    }
}