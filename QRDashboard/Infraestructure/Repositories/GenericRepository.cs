using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Infraestructure.Data;

namespace QRDashboard.Infraestructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> Obtain(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entity = await _context.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entity;
            }
            catch
            {
                throw;
            }
        }
        public Task<IQueryable<TEntity>> Consult(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> queryEntity = filtro == null? _context.Set<TEntity>() : _context.Set<TEntity>().Where(filtro);
            return Task.FromResult(queryEntity);
        }
        
        public async Task<TEntity> Create(TEntity entidad)
        {
            try
            {
                _context.Set<TEntity>().Add(entidad);
                await _context.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(TEntity entidad)
        {
            try
            {
                _context.Set<TEntity>().Update(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminate(TEntity entidad)
        {
            try
            {
                _context.Set<TEntity>().Remove(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EliminateRange<TEntidad>(Expression<Func<TEntity, bool>> filtro) where TEntidad : class
        {
            try
            {
                var rangeDelete = _context.Set<TEntity>().Where(filtro);
                _context.Set<TEntity>().RemoveRange(rangeDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}