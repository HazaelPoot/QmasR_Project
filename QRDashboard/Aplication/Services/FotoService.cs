using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Aplication.Services
{
    public class FotoService : IFotoService
    {
        private readonly IGenericRepository<FotosProyecto> _repository;
        private readonly IFirebaseService _fireBaseService;

        public FotoService(IGenericRepository<FotosProyecto> repository, IFirebaseService fireBaseService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
        }
        public async Task<List<FotosProyecto>> Lista()
        {
            IQueryable<FotosProyecto> query = await _repository.Consult();
            return query.Include(r => r.IdProjNavigation).ToList();
        }

        public async Task<List<FotosProyecto>> ListByProject(int idProj)
        {
            IQueryable<FotosProyecto> query = await _repository.Consult(u => u.IdProj == idProj);
            return query.Include(r => r.IdProjNavigation).ToList();
        }

        public async Task<FotosProyecto> GetById(int idImage)
        {
            IQueryable<FotosProyecto> query = await _repository.Consult(u => u.IdImg == idImage);
            FotosProyecto result = query.Include(r => r.IdProjNavigation).FirstOrDefault();

            return result;
        }
        
        public async Task<FotosProyecto> Crear(FotosProyecto entidad, int? idProj, Stream Foto = null, string carpetaDestino = "", string NombreFoto = "")
        {
            FotosProyecto fotoExist = await _repository.Obtain(u => u.IdImg == entidad.IdImg || u.NombreFoto == entidad.NombreFoto);

            if(fotoExist != null)
                throw new TaskCanceledException($"Ya existe una foto con el nombre {fotoExist.NombreFoto}");

            try
            {
                entidad.IdProj = idProj;
                entidad.NombreFoto = NombreFoto;
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.UploadStorage(Foto, carpetaDestino, NombreFoto);
                    entidad.UrlImage = urlFoto;
                }

                FotosProyecto fotoCreado = await _repository.Create(entidad);
                if(fotoCreado.IdImg == 0)
                    throw new TaskCanceledException("No se pudo subir la foto");

                IQueryable<FotosProyecto> query = await _repository.Consult(u => u.IdImg == fotoCreado.IdImg);
                fotoCreado = query.First();

                return fotoCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idImage)
        {
            try
            {
                FotosProyecto fotoEncontrado = await _repository.Obtain(u => u.IdImg == idImage);

                if(fotoEncontrado == null)
                throw new TaskCanceledException("El Proyecto no existe");

                string nombreFoto = fotoEncontrado.NombreFoto;
                bool response = await _repository.Eliminate(fotoEncontrado);

                if(response)
                    await _fireBaseService.DeleteStorage("Fotos_Imagen", nombreFoto);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EliminarList(int idProj)
        {
            try
            {
                IQueryable<FotosProyecto> fotoEncontrado = await _repository.Consult(u => u.IdProj == idProj);
                List<FotosProyecto> fotos = fotoEncontrado.ToList();

                if(fotos == null)
                    throw new TaskCanceledException("Las fotos no existen");

                foreach (var item in fotos)
                {
                    await Eliminar(item.IdImg);
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}