using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using LibraryManagementSystem.DAL.SpecificationContracts;
using LibraryManagementSystem.DAL.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity

    {
        private readonly LibraryDbContext dbContext;

        public GenericRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        {
            return withTracking ? await dbContext.Set<TEntity>().ToListAsync()
                            : await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }


        public async Task<TEntity?> GetByIdAsync(int id)
        {

            //if (typeof(TEntity) == typeof(Product))
            //    return await dbContext.Set<Product>().Where(p => p.Id.Equals(id)).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as TEntity;

            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
            => await dbContext.Set<TEntity>().AddAsync(entity);


      
            //=> dbContext.Set<TEntity>().Update(entity);



        public bool Delete(TEntity entity)
        {

            if (entity == null)
            {
                return false;
            }

            dbContext.Set<TEntity>().Remove(entity);

            return true;
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> spec, bool withTracking = false)
        {
            return await SpcificationsEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>(), spec).ToListAsync();
        }

        public async Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity> spec)
        {
            return await SpcificationsEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>(), spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<TEntity> spec)
        {
            return await SpcificationsEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>(), spec).CountAsync();
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().AnyAsync(predicate);
        }

        bool IGenericRepository<TEntity>.Update(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            dbContext.Set<TEntity>().Update(entity);

            return true;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
                return await dbContext.Set<TEntity>().CountAsync(predicate);
            return await dbContext.Set<TEntity>().CountAsync();
        }

    }
}
