using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Aplication.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IGenericRepository<ProyectoQr> _repositorio;
        private readonly IFirebaseService _fireBaseService;

        public ProyectoService(IGenericRepository<ProyectoQr> repositorio, IFirebaseService fireBaseService)
        {
            _repositorio = repositorio;
            _fireBaseService = fireBaseService;
        }

        public async Task<List<ProyectoQr>> Lista()
        {
            IQueryable<ProyectoQr> query = await _repositorio.Consult();
            return query.Include(r => r.IdCategoriaNavigation).ToList();
        }
        public async Task<ProyectoQr> Crear(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            ProyectoQr projectExist = await _repositorio.Obtain(u => u.Titulo == entidad.Titulo);

            if(projectExist != null)
                throw new TaskCanceledException("Ya existe un proyecto con ese nombre");

            try
            {
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    entidad.UrlImagen = urlFoto;
                }

                ProyectoQr projectCreado = await _repositorio.Create(entidad);
                if(projectCreado.IdProj == 0)
                    throw new TaskCanceledException("No se pudo crear el Proyecto");

                IQueryable<ProyectoQr> query = await _repositorio.Consult(u => u.IdProj == projectCreado.IdProj);
                projectCreado = query.First();

                return projectCreado;

            }
            catch
            {
                throw;
            }
        }

        public Task<ProyectoQr> Editar(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int idProyecto)
        {
            throw new NotImplementedException();
        }

        public Task<ProyectoQr> ObtenerPorCredenciales(string username, string clave)
        {
            throw new NotImplementedException();
        }

        public Task<ProyectoQr> ObtenerPorId(int idProyecto)
        {
            throw new NotImplementedException();
        }
    }
}