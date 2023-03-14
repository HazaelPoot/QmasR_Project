using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> Crear(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<Usuario> Editar(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<bool> Eliminar(int IdUsuario);
        Task<Usuario> Autenthication(string username, string password);
        Task<Usuario> GetById (int IdUsuario);
        Task<bool> GuardarPerfil(Usuario entidad);
    }
}