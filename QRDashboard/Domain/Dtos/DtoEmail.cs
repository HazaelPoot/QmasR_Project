namespace QRDashboard.Domain.Dtos
{
    public class DtoEmail
    {
        public string Remitemte { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
    }
}