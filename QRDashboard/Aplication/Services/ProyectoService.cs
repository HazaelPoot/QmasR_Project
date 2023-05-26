using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Aplication.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IGenericRepository<ProyectoQr> _repository;
        private readonly IFirebaseService _fireBaseService;
        private readonly IFotoService _fotoService;

        public ProyectoService(IGenericRepository<ProyectoQr> repository, IFirebaseService fireBaseService, IFotoService fotoService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
            _fotoService = fotoService;
        }

        public async Task<List<ProyectoQr>> Lista()
        {
            IQueryable<ProyectoQr> query = await _repository.Consult();
            return query.Include(r => r.IdCategoriaNavigation).ToList();
        }

        public async Task<List<ProyectoQr>> ListActivate()
        {
            IQueryable<ProyectoQr> query = await _repository.Consult(u => u.Status == 0);
            return query.Include(r => r.IdCategoriaNavigation).ToList();
        }

        public async Task<ProyectoQr> GetById(int IdProj)
        {
            IQueryable<ProyectoQr> query = await _repository.Consult(u => u.IdProj == IdProj);
            ProyectoQr resultado = query.Include(r => r.IdCategoriaNavigation).FirstOrDefault();

            return resultado;
        }
        
        public async Task<ProyectoQr> Crear(ProyectoQr entidad, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            ProyectoQr projectExist = await _repository.Obtain(u => u.Titulo == entidad.Titulo);

            if(projectExist != null)
                throw new TaskCanceledException($"Ya existe un Proyecto con el titulo {projectExist.Titulo}");

            try
            {
                entidad.NombreFoto = NombreFoto;
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    entidad.UrlImagen = urlFoto;
                }

                ProyectoQr projectCreado = await _repository.Create(entidad);
                if(projectCreado.IdProj == 0)
                    throw new TaskCanceledException("No se pudo crear el Proyecto");

                IQueryable<ProyectoQr> query = await _repository.Consult(u => u.IdProj == projectCreado.IdProj);
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
            ProyectoQr proyectExist = await _repository.Obtain(u => u.Titulo == entidad.Titulo && u.IdProj != entidad.IdProj);

            if (proyectExist != null)
                throw new TaskCanceledException("El Proyecto ya existe");

            try
            {
                IQueryable<ProyectoQr> queryProj = await _repository.Consult(u => u.IdProj == entidad.IdProj);
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

                bool response = await _repository.Edit(proj_editar);

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
                ProyectoQr proyectoEncontrado = await _repository.Obtain(u => u.IdProj == idProyecto);

                if(proyectoEncontrado == null)
                    throw new TaskCanceledException("El Proyecto no existe");

                await _fotoService.EliminarList(idProyecto);
                
                string nombreFoto = proyectoEncontrado.NombreFoto;
                bool response = await _repository.Eliminate(proyectoEncontrado);

                if(response)
                    await _fireBaseService.DeleteStorage("Fotos_Proyecto", nombreFoto);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}