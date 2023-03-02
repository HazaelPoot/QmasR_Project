namespace QRDashboard.Domain.Entities
{
    public partial class FotosProyecto
    {
        public int IdImg { get; set; }
        public string UrlImage { get; set; } = null!;
        public int? IdProj { get; set; }

        public virtual ProyectoQr? IdProjNavigation { get; set; }
    }
}
