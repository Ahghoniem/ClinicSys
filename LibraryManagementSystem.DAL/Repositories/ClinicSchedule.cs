using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class ClinicSchedule<TEntity> : IClinicSchedule<TEntity> where TEntity : class
    {

        private readonly LibraryDbContext dbContext;

        public ClinicSchedule(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        public async Task AddSchedule(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            
        }
        public async Task AddSchedules(List<TEntity> schedule)
        {
            await dbContext.Set<TEntity>().AddRangeAsync(schedule);

        }

        public async Task<IEnumerable<ClinicAndUsers>> GetAllSchedule()
        {
            var Schedule = await (from schedule in dbContext.Clinic_schedule
                                  join user in dbContext.Users on schedule.DID equals user.Id 
                                  join department in dbContext.Department on schedule.DepID equals department.Id
                                  select new ClinicAndUsers { schedule = schedule, user = user , Department =department }).ToListAsync();
            
            
            return Schedule;
        }
        public async Task<IEnumerable<ClinicAndUsers>> GetAllDepSchedule(int Id)
        {
            var Schedule = await (from schedule in dbContext.Clinic_schedule
                                  join user in dbContext.Users on schedule.DID equals user.Id
                                  join department in dbContext.Department on schedule.DepID equals department.Id
                                  where (schedule.DepID == Id)
                                  select new ClinicAndUsers { schedule = schedule, user = user, Department = department }).ToListAsync();


            return Schedule;
        }
        public async Task<IEnumerable<ClinicAndUsers>> GetAllDoctorSchedule2(String Id, string day, int shift)
        {
            var Schedule = await (from schedule in dbContext.Clinic_schedule
                                  join user in dbContext.Users on schedule.DID equals user.Id
                                  join department in dbContext.Department on schedule.DepID equals department.Id
                                  where (schedule.DID == Id)
                                  where (schedule.day == day)
                                  where (schedule.shift == shift)
                                  select new ClinicAndUsers { schedule = schedule, user = user, Department = department }).ToListAsync();


            return Schedule;
        }

        public async Task<IEnumerable<ClinicAndUsers>> GetAllDoctorSchedule(String Id)
        {
            var Schedule = await (from schedule in dbContext.Clinic_schedule
                                  join user in dbContext.Users on schedule.DID equals user.Id
                                  join department in dbContext.Department on schedule.DepID equals department.Id
                                  where (schedule.DID == Id)
                                  select new ClinicAndUsers { schedule = schedule, user = user, Department = department }).ToListAsync();


            return Schedule;
        }
        public async Task<IEnumerable<DoctorPatient>> GetAllDoctorPatient(String Id)
        {
            var Schedule = await (from schedule in dbContext.Patient_schedule
                                  join user in dbContext.Patients on schedule.PID equals user.Id
                                  where (schedule.DID == Id  && schedule.status == "Approved")
                                  select new DoctorPatient {  user = user }).ToListAsync();


            return Schedule;
        }

        public async Task<bool> update(TEntity entity)
        {

            if (entity == null)
            {
                return false;
            }

            dbContext.Set<TEntity>().Update(entity);

            return true;
        }
        public async Task<bool> updateRange(List<TEntity> entitys)
        {

            if (entitys == null)
            {
                return false;
            }

            dbContext.Set<TEntity>().UpdateRange(entitys);

            return true;
        }
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

    }
}
