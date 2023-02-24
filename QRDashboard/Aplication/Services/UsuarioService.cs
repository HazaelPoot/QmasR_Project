using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Entities;
using System.Net;
using System.Text;

namespace QRDashboard.Aplication.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IFirebaseService _fireBaseService;

        public UsuarioService(IGenericRepository<Usuario> repositorio, IFirebaseService fireBaseService)
        {
            _repositorio = repositorio;
            _fireBaseService = fireBaseService;
        }

        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repositorio.Consult();
            return query.Include(r => r.AdminTypeNavigation).ToList(); 
        }

        public async Task<Usuario> Crear(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            Usuario usuarioExist = await _repositorio.Obtain(u => u.Username == entidad.Username);

            if(usuarioExist != null)
                throw new TaskCanceledException("El Usuario ya existe");

            try
            {
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    entidad.UrlImagen = urlFoto;
                }

                Usuario usuarioCreado = await _repositorio.Create(entidad);
                if(usuarioCreado.IdUser == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                IQueryable<Usuario> query = await _repositorio.Consult(u => u.IdUser == usuarioCreado.IdUser);
                usuarioCreado = query.Include(r => r.AdminTypeNavigation).First();

                return usuarioCreado;

            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> Editar(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            Usuario usuarioExist = await _repositorio.Obtain(u => u.Username == entidad.Username && u.IdUser != entidad.IdUser);

            if (usuarioExist != null)
                throw new TaskCanceledException("El Usuario ya existe");

            try
            {
                IQueryable<Usuario> queryUser = await _repositorio.Consult(u => u.IdUser == entidad.IdUser);
                Usuario usuario_editar = queryUser.First();
                usuario_editar.Nombre = entidad.Nombre;
                usuario_editar.Apellidos = entidad.Apellidos;
                usuario_editar.Username = entidad.Username;
                usuario_editar.Contraseña = entidad.Contraseña;
                usuario_editar.AdminType = entidad.AdminType;

                if(usuario_editar.UrlImagen == "")
                    usuario_editar.UrlImagen = entidad.UrlImagen;

                if (Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    usuario_editar.UrlImagen = urlFoto;
                }

                bool response = await _repositorio.Edit(usuario_editar);

                if(!response)
                    throw new TaskCanceledException("No se pudo moficiar el usuario");

                Usuario usuario_editado = queryUser.Include(r => r.AdminTypeNavigation).First();

                return usuario_editado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int IdUsuario)
        {
            try
            {
                Usuario usuarioEncontrado = await _repositorio.Obtain(u => u.IdUser == IdUsuario);

                if(usuarioEncontrado == null)
                throw new TaskCanceledException("El Usuario no existe");

                string nombreFoto = usuarioEncontrado.UrlImagen;
                bool response = await _repositorio.Eliminate(usuarioEncontrado);

                if(response)
                    await _fireBaseService.DeleteStorage("Fotos_Perfil", nombreFoto);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public Task<Usuario> ObtenerPorCredenciales(string username, string clave)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ObtenerPorId(int IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GuardarPerfil(Usuario entidad)
        {
            throw new NotImplementedException();
        }
    }
}