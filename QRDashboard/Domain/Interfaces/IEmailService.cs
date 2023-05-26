using QRDashboard.Domain.Dtos;

namespace QRDashboard.Domain.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(DtoEmail email);
    }
}