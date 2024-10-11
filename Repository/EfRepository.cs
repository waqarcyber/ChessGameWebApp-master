using DbContextDao;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly AuthContext _dbContext;

        public EfRepository(AuthContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<TEntity> _entities => _dbContext.Set<TEntity>();

        public virtual Task<TEntity> GetById(int id)
        {
            var result = _entities.First(it => it.Id == id);
            return Task.FromResult(result);
        }

        public virtual Task<IReadOnlyList<TEntity>> GetAll()
        {
            IReadOnlyList<TEntity> result = _entities.ToList();
            return Task.FromResult(result);
        }

        public virtual Task Add(TEntity entity)
        {
            _entities.Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task Update(TEntity TEntity)
        {
            _dbContext.Entry(TEntity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task Remove(TEntity entity)
        {
            _entities.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<TEntity?> FindById(int id)
        {
            var result = _entities.FirstOrDefault(it => it.Id == id);
            return Task.FromResult(result);
        }
    }
}