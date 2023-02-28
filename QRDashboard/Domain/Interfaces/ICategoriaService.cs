using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categorium>> Lista();
    }
}