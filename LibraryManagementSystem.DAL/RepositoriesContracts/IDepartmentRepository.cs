using LibraryManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.RepositoriesContracts
{
     public interface IDepartmentRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllDep();
        Task<TEntity> GetDepById(int id);
        Task<TEntity> GetDepById(int? id);
        

    }
}
