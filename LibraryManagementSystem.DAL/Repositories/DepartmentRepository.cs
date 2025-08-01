using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class DepartmentRepository<TEntity> : IDepartmentRepository<TEntity> where TEntity : BaseEntity
    {


        private readonly LibraryDbContext dbContext;

        public DepartmentRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<TEntity>> GetAllDep()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetDepById(int id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }public async Task<TEntity> GetDepById(int? id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }
    }
}
