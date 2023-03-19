using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface IFotoService
    {
        Task<List<FotosProyecto>> Lista();
        Task<List<FotosProyecto>> ListByProject(int idProj);
        Task<FotosProyecto> GetById (int idImage);
        Task<FotosProyecto> Crear(FotosProyecto entidad, int? idProj, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<bool> Eliminar(int idImage);
    }
}