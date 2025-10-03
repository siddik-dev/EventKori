using EventKori.Domain.Entities;
using EventKori.Domain.Interfaces;
using EventKori.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly EventKoriDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(EventKoriDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => e.IsActive).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(e => e.IsActive).Where(predicate).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            entity.IsActive = false;
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync(e => e.IsActive);

            return await _dbSet.Where(e => e.IsActive).CountAsync(predicate);
        }
    }
}
