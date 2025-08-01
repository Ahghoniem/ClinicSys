using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.SpecificationContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.RepositoriesContracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
       
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> spec, bool withTracking = false);

        Task<TEntity?> GetByIdAsync(int id); 
        Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity> spec);

        Task AddAsync(TEntity entity);

        bool Update(TEntity entity);
        bool Delete(TEntity entity);

        Task<int> GetCountAsync(ISpecifications<TEntity> spec);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    }
}
