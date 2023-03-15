namespace QRDashboard.Domain.Entities
{
    public partial class Usuario
    {
        public int IdUser { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Username { get; set; }
        public string Passw { get; set; }
        public string NombreFoto { get; set; }
        public string UrlImagen { get; set; }
        public int AdminType { get; set; }

        public virtual AdminType AdminTypeNavigation { get; set; }
    }
}
