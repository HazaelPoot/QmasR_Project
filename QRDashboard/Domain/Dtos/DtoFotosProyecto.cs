namespace QRDashboard.Domain.Dtos
{
    public class DtoFotosProyecto
    {
        public int IdImg { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImage { get; set; }
        public int? IdProj { get; set; }
        public string NombreProyecto { get; set; }
    }
}