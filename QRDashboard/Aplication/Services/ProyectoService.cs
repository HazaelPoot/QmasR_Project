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

        public async Task<ProyectoQr> GetById(int IdProj)
        {
            IQueryable<ProyectoQr> query = await _repositorio.Consult(u => u.IdProj == IdProj);
            ProyectoQr resultado = query.Include(r => r.IdCategoriaNavigation).FirstOrDefault();

            return resultado;
        }
        
        public async Task<ProyectoQr> Crear(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            ProyectoQr projectExist = await _repositorio.Obtain(u => u.Titulo == entidad.Titulo);

            if(projectExist != null)
                throw new TaskCanceledException("Ya existe un Proyecto con ese Titulo");

            try
            {
                entidad.NombreFoto = NombreFoto;
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

        public async Task<ProyectoQr> Editar(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            ProyectoQr proyectExist = await _repositorio.Obtain(u => u.Titulo == entidad.Titulo && u.IdProj != entidad.IdProj);

            if (proyectExist != null)
                throw new TaskCanceledException("El Proyecto ya existe");

            try
            {
                IQueryable<ProyectoQr> queryProj = await _repositorio.Consult(u => u.IdProj == entidad.IdProj);
                ProyectoQr proj_editar = queryProj.First();
                proj_editar.Titulo = entidad.Titulo;
                proj_editar.Descripcion = entidad.Descripcion;
                proj_editar.Ubicacion = entidad.Ubicacion;
                proj_editar.Presupuesto = entidad.Presupuesto;
                proj_editar.IdCategoria = entidad.IdCategoria;
                proj_editar.Status = entidad.Status;

                if(proj_editar.NombreFoto == "")
                    proj_editar.UrlImagen = NombreFoto;

                if (Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    proj_editar.UrlImagen = urlFoto;
                }

                bool response = await _repositorio.Edit(proj_editar);

                if(!response)
                    throw new TaskCanceledException("No se pudo moficiar el Proyecto");

                ProyectoQr proj_editado = queryProj.Include(r => r.IdCategoriaNavigation).First();

                return proj_editado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProyecto)
        {
            try
            {
                ProyectoQr proyectoEncontrado = await _repositorio.Obtain(u => u.IdProj == idProyecto);

                if(proyectoEncontrado == null)
                throw new TaskCanceledException("El Proyecto no existe");

                string nombreFoto = proyectoEncontrado.NombreFoto;
                bool response = await _repositorio.Eliminate(proyectoEncontrado);

                if(response)
                    await _fireBaseService.DeleteStorage("Fotos_Proyecto", nombreFoto);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public Task<ProyectoQr> ObtenerPorCredenciales(string username, string clave)
        {
            throw new NotImplementedException();
        }
    }
}