using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categorium>> Lista();
        Task<Categorium> Crear(Categorium entidad);
        Task<Categorium> Editar(Categorium entidad);
        Task<bool> Eliminar(int IdCategoria);
        Task<Categorium> GetById (int IdCategoria);
    }
}