using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Entities;

namespace QRDashboard.Aplication.Services
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<AdminType> _repository;
        public RolService(IGenericRepository<AdminType> repository)
        {
            _repository = repository;
        }
        public async Task<List<AdminType>> Lista()
        {
            IQueryable<AdminType> query = await _repository.Consult();
            return query.ToList();
        }
    }
}