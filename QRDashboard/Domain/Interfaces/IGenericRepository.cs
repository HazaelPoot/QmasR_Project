using System.Linq.Expressions;

namespace QRDashboard.Domain.Interfaces
{
    public interface IGenericRepository <TEntity> where TEntity : class
    {
        Task<TEntity> Obtain(Expression<Func<TEntity, bool>> filtro);
        Task<TEntity> Create(TEntity entidad);
        Task<bool> Edit(TEntity entidad);
        Task<bool> Eliminate(TEntity entidad);
        Task<IQueryable<TEntity>> Consult(Expression<Func<TEntity, bool>> filtro = null);
    }
}