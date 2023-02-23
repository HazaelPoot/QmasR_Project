namespace QRDashboard.Domain.Dtos.Response
{
    public class GenericResponse <TObejct>
    {
        public bool Status { get; set; }
        public string? Mesaje { get; set; }
        public TObejct? Obejct { get; set; }
        public List<TObejct>? ListObject { get; set; }
    }
}