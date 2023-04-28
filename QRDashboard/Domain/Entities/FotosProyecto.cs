namespace QRDashboard.Domain.Entities
{
    public partial class FotosProyecto
    {
        public int IdImg { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImage { get; set; }
        public int? IdProj { get; set; }

        public virtual ProyectoQr IdProjNavigation { get; set; }
    }
}