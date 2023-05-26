namespace QRDashboard.Domain.Entities
{
    public partial class ProyectoQr
    {
        public ProyectoQr()
        {
            FotosProyectos = new HashSet<FotosProyecto>();
        }

        public int IdProj { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public decimal? Presupuesto { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImagen { get; set; }
        public int Status { get; set; }
        public int? IdCategoria { get; set; }

        public virtual Categorium IdCategoriaNavigation { get; set; }
        public virtual ICollection<FotosProyecto> FotosProyectos { get; set; }
    }
}