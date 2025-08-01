using LibraryManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.RepositoriesContracts
{
    public interface IClinicSchedule<TEntity> where TEntity : class
    {
        Task<IEnumerable<ClinicAndUsers>> GetAllSchedule();
        public Task<IEnumerable<ClinicAndUsers>> GetAllDoctorSchedule2(String Id, string day, int shift);

        Task AddSchedule(TEntity entity);
        Task AddSchedules(List<TEntity> schedule);
        Task<IEnumerable<ClinicAndUsers>> GetAllDepSchedule(int Id);
        Task<IEnumerable<ClinicAndUsers>> GetAllDoctorSchedule(String Id);
        
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<DoctorPatient>> GetAllDoctorPatient(String Id);
        Task<bool> update(TEntity entity);
        Task<bool> updateRange(List<TEntity> entitys);







    }
}
