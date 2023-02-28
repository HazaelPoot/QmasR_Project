namespace QRDashboard.Models
{
    public class VMProyecto
    {
        public int IdProj { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime FinaliDatre { get; set; }
        public string Ubicacion { get; set; } = null!;
        public decimal Presupuesto { get; set; }
        public int? IdCategoria { get; set; }
        public string CategoriaName { get; set; } = null!;
        public int Status { get; set; } = 0;
        public string UrlImagen { get; set; } = null!;
    }
}