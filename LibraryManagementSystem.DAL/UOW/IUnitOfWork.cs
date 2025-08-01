using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.UOW
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>()
             where TEntity : BaseEntity;

        IClinicSchedule<TEntity> GetClinicRepository<TEntity>()
            where TEntity : class;

        IPatientSchedule<TEntity> GetPatientRepository<TEntity>()
             where TEntity : class;
        IConversationRepository Conversations { get; }

        public Task<int> SaveAsync();
    }
}
