using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using System.Collections.Concurrent;

namespace LibraryManagementSystem.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _dbContext;

        private readonly ConcurrentDictionary<string, object> _repositories;
        public IConversationRepository Conversations { get; }


        public UnitOfWork(LibraryDbContext dbcontect)
        {
            _dbContext = dbcontect;
            _repositories = new ConcurrentDictionary<string, object>();
            Conversations = new ConversationRepository(_dbContext);
        }



        public async Task<int> SaveAsync()
             => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity

        {


            //var typeName = typeof(TEntity).Name;

            //if (_repositories.ContainsKey(typeName)) 
            //    return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            //var repo = new GenericRepository<TEntity, TKey>(_dbContext);
            //_repositories.AddOrUpdate(typeName, repo);
            //return repo;


            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity>(_dbContext));
        }

        public IClinicSchedule<TEntity> GetClinicRepository<TEntity>() where TEntity : class

        {


            //var typeName = typeof(TEntity).Name;

            //if (_repositories.ContainsKey(typeName)) 
            //    return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            //var repo = new GenericRepository<TEntity, TKey>(_dbContext);
            //_repositories.AddOrUpdate(typeName, repo);
            //return repo;


            return (IClinicSchedule<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new ClinicSchedule<TEntity>(_dbContext));
        }
        public IPatientSchedule<TEntity> GetPatientRepository<TEntity>() where TEntity : class

        {


            //var typeName = typeof(TEntity).Name;

            //if (_repositories.ContainsKey(typeName)) 
            //    return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            //var repo = new GenericRepository<TEntity, TKey>(_dbContext);
            //_repositories.AddOrUpdate(typeName, repo);
            //return repo;


            return (IPatientSchedule<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new PatientScheduleRepository<TEntity>(_dbContext));
        }
    }
}

