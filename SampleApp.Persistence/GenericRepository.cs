using System;
using System.Collections.Generic;
using System.Linq;
using SampleApp.Persistence.Infrastructure;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;

namespace SampleApp.Persistence
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        #region Members

        private readonly SampleAppDbContext _dbContext;

        #endregion

        #region Ctor

        public GenericRepository(SampleAppDbContext dbContext) => _dbContext = dbContext;

        #endregion

        public async Task CreateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            entity.CreatedAt = DateTime.Now;
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task CreateBulkAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.Now;
            }
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            entity.ModifiedAt = DateTime.Now;
            entity.IsDeleted = true;
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
                entity.ModifiedAt = DateTime.Now;
                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> Filter(Func<TEntity, bool> predicate)
        {
            var entities = _dbContext.Set<TEntity>().AsNoTracking().AsEnumerable().Where(predicate);
            return await Task.FromResult(entities.ToList());
        }

        public async Task<TEntity> GetById(string id)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(q => q.Id.Equals(id) && !q.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            entity.ModifiedAt = DateTime.Now;
            _dbContext.Set<TEntity>().Update(entity);
            await SaveChangesAsync();
        }

        #region Dispose

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}