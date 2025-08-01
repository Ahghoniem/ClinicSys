using LibraryManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.RepositoriesContracts
{
    public interface IPatientSchedule<TEntity> where TEntity : class
    {
        Task<List<Patient_schedule>> GetByIdAsync2(String id);
        Task<List<Clinic_schedule>> GetclinicSchedulebyDoctorIdAsync(String id);
        Task<IEnumerable<usersAndSchedule>> GetAllAppointment();

        Task<IEnumerable<usersAndSchedule>> GetpatientScheduleById(String Id);
        Task<IEnumerable<usersAndSchedule>> GetClinicScheduleById(int Id);
        public Task<IEnumerable<usersAndSchedule>> GetclinicScheduleById(int Id);


        Task<IEnumerable<usersAndSchedule>> GetDoctorScheduleById(String Id);

        Task<IEnumerable<usersAndSchedule>> GetDepartmentScheduleById(int Id);

        Task<bool> update(TEntity entity);
        Task<bool> updateRange(List<TEntity> entitys);

        Task BookAppointment(TEntity entity);

        Task<TEntity?> GetByIdAsync(String id);
        Task<TEntity?> GetByIdAsync(int id);
        Task<Dictionary<int, int>> GetMonthlyPatientScheduleCountAsync();
        Task<Dictionary<int, Dictionary<int, int>>> GetMonthlyPatientCountPerDepartmentAsync(int year);
        bool Delete(TEntity entity);

        bool DeleteRange(List<TEntity> entitys);
        Task<IEnumerable<usersAndSchedule>> GetpatientScheduleByIdAndSId(string PId, int SId);



    }
}

