namespace QRDashboard.Domain.Entities
{
    public partial class ProyectoQr
    {
        public ProyectoQr()
        {
            FotosProyectos = new HashSet<FotosProyecto>();
        }

        public int IdProj { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime FinaliDatre { get; set; }
        public string Ubicacion { get; set; } = null!;
        public decimal Presupuesto { get; set; }
        public string UrlImagen { get; set; } = null!;
        public int Status { get; set; }
        public int? IdCategoria { get; set; }

        public virtual Categorium? IdCategoriaNavigation { get; set; }
        public virtual ICollection<FotosProyecto> FotosProyectos { get; set; }
    }
}
