using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface IAccesoService
    {
        Task<Usuario> Authentication(string username, string password);
        Task<Usuario> GetUserAuth (int IdUsuario);
        Task<bool> GenerateClaims(DtoUsuario user);
    }
}