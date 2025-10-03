using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventKori.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly EventKoriDbContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(EventKoriDbContext dbContext)
        {
            _context = dbContext;
            _entities = _context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _entities.Where(e => e.IsActive).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(e => e.IsActive).Where(predicate).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            entity.IsActive = false;
            _entities.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _entities.AnyAsync(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _entities.CountAsync(e => e.IsActive);
            }

            return await _entities.Where(e => e.IsActive).CountAsync(predicate);
        }
    }
}
