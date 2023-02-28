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
    }
}