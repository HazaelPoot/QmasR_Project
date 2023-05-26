using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface IProyectoService
    {
        Task<List<ProyectoQr>> Lista();
        Task<List<ProyectoQr>> ListActivate();
        Task<ProyectoQr> GetById (int idProyecto);
        Task<ProyectoQr> Crear(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<ProyectoQr> Editar(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<bool> Eliminar(int idProyecto);
    }
}