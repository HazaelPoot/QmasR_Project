namespace QRDashboard.Domain.Dtos
{
    public class DtoUsuario
    {
        public int IdUser { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Passw { get; set; } = null!;
        public int? AdminType { get; set; }
        public string AdminTypeName { get; set; } = null!;
        public string UrlImagen { get; set; } = null!;
    }
}