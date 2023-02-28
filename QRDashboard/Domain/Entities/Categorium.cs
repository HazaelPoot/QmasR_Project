namespace QRDashboard.Domain.Entities
{
    public partial class Categorium
    {
        public Categorium()
        {
            ProyectoQrs = new HashSet<ProyectoQr>();
        }

        public int IdCategoria { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<ProyectoQr> ProyectoQrs { get; set; }
    }
}
