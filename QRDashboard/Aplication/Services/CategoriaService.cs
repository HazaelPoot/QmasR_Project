using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Aplication.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categorium> _repository;
        public CategoriaService(IGenericRepository<Categorium> repository)
        {
            _repository = repository;
        }

        public async Task<List<Categorium>> Lista()
        {
            IQueryable<Categorium> query = await _repository.Consult();
            return query.ToList();
        }

        public async Task<Categorium> GetById(int IdCategoria)
        {
            IQueryable<Categorium> query = await _repository.Consult(u => u.IdCategoria == IdCategoria);
            Categorium resultado = query.FirstOrDefault();

            return resultado;
        }
        
        public async Task<Categorium> Crear(Categorium entidad)
        {
            try
            {
                Categorium catgCreado = await _repository.Create(entidad);
                if(catgCreado.IdCategoria == 0)
                    throw new TaskCanceledException("No se pudo crear la categoria");

                return catgCreado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Categorium> Editar(Categorium entidad)
        {
            try
            {
                Categorium category_encontrada = await _repository.Obtain(c => c.IdCategoria == entidad.IdCategoria);
                category_encontrada.Descripcion = entidad.Descripcion;

                bool response = await _repository.Edit(category_encontrada);

                if(!response)
                    throw new TaskCanceledException("No se pudo moficiar la categoria");

                return category_encontrada;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int IdCategoria)
        {
            try
            {
                Categorium catgEncontrado = await _repository.Obtain(u => u.IdCategoria == IdCategoria);

                if(catgEncontrado == null)
                    throw new TaskCanceledException("La Categoria no existe");

                bool response = await _repository.Eliminate(catgEncontrado);

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}