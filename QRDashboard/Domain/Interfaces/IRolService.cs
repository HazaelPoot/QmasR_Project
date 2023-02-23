using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface IRolService
    {
        Task<List<AdminType>> Lista();
    }
}