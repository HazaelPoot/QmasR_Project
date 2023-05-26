using QRDashboard.Domain.Entities;

namespace QRDashboard.Domain.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> GetById (int IdUsuario);
        Task<Usuario> Crear(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<Usuario> Editar(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto ="");
        Task<bool> Eliminar(int IdUsuario);
        Task<bool> GuardarPerfil(Usuario entidad);
        Task<bool> CambiarClave(int IdUsuario, string claveActual, string claveNueva);
    }
}