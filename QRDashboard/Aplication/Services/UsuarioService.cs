using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Aplication.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IFirebaseService _fireBaseService;
        private readonly IUtilityService _utilityService;

        public UsuarioService(IGenericRepository<Usuario> repository, IFirebaseService firebaseService, IUtilityService utilityService)
        {
            _repository = repository;
            _fireBaseService = firebaseService;
            _utilityService = utilityService;
        }

        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repository.Consult();
            return query.Include(r => r.AdminTypeNavigation).ToList(); 
        }

        public async Task<Usuario> GetById(int IdUsuario)
        {
            IQueryable<Usuario> query = await _repository.Consult(u => u.IdUser == IdUsuario);
            Usuario result = query.Include(r => r.AdminTypeNavigation).FirstOrDefault();

            return result;
        }

        public async Task<Usuario> Crear(Usuario entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            Usuario usuarioExist = await _repository.Obtain(u => u.Username == entidad.Username);

            if(usuarioExist != null)
                throw new TaskCanceledException($"Ya existe un Usuario con el username {usuarioExist.Username}");

            try
            {
                string pswEncript = _utilityService.EncryptMD5(entidad.Passw);
                entidad.Passw = pswEncript;
                
                entidad.NombreFoto = NombreFoto;
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    entidad.UrlImagen = urlFoto;
                }

                Usuario usuarioCreado = await _repository.Create(entidad);
                if(usuarioCreado.IdUser == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                IQueryable<Usuario> query = await _repository.Consult(u => u.IdUser == usuarioCreado.IdUser);
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
            Usuario usuarioExist = await _repository.Obtain(u => u.Username == entidad.Username && u.IdUser != entidad.IdUser);

            if (usuarioExist != null)
                throw new TaskCanceledException("El Usuario No existe");

            try
            {
                IQueryable<Usuario> queryUser = await _repository.Consult(u => u.IdUser == entidad.IdUser);
                Usuario usuario_editar = queryUser.First();
                usuario_editar.Nombre = entidad.Nombre;
                usuario_editar.Apellidos = entidad.Apellidos;
                usuario_editar.Username = entidad.Username;
                usuario_editar.Passw = entidad.Passw;
                usuario_editar.AdminType = entidad.AdminType;

                if(usuario_editar.NombreFoto == "")
                    usuario_editar.NombreFoto = NombreFoto;

                if (Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    usuario_editar.UrlImagen = urlFoto;
                }

                bool response = await _repository.Edit(usuario_editar);

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
                Usuario usuarioEncontrado = await _repository.Obtain(u => u.IdUser == IdUsuario);

                if(usuarioEncontrado == null)
                throw new TaskCanceledException("El Usuario no existe");

                string nombreFoto = usuarioEncontrado.NombreFoto;
                bool response = await _repository.Eliminate(usuarioEncontrado);

                if(response)
                    await _fireBaseService.DeleteStorage("Fotos_Perfil", nombreFoto);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> GuardarPerfil(Usuario entidad)
        {
            try
            {
                Usuario user_encontrado = await _repository.Obtain(u => u.IdUser == entidad.IdUser);

                if(user_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");
                
                user_encontrado.Nombre = entidad.Nombre;
                user_encontrado.Apellidos = entidad.Apellidos;
                user_encontrado.Username = entidad.Username;
                
                bool response = await _repository.Edit(user_encontrado);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CambiarClave(int IdUsuario, string claveActual, string claveNueva)
        {
            try
            {
                Usuario user_encontrado = await _repository.Obtain(u => u.IdUser == IdUsuario);
                string claveUser = _utilityService.DesencryptMD5(user_encontrado.Passw);

                if(user_encontrado is null)
                    throw new TaskCanceledException("El usuario no existe");

                if(claveUser != claveActual)
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");

                user_encontrado.Passw = _utilityService.EncryptMD5(claveNueva);

                bool response = await _repository.Edit(user_encontrado);

                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}