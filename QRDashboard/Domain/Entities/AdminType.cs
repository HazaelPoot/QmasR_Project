namespace QRDashboard.Domain.Entities
{
    public class AdminType
    {
        public AdminType()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string Tipo { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}