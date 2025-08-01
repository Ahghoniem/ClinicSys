using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.DAL.Repositories
{
    public class PatientScheduleRepository<TEntity> : IPatientSchedule<TEntity> where TEntity : class
    {
        private readonly LibraryDbContext dbContext;

        public PatientScheduleRepository(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TEntity?> GetByIdAsync(String id)
        {
      
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<Patient_schedule>> GetByIdAsync2(String id)
        {

            return await dbContext.Patient_schedule.Where(x => x.DID == id).ToListAsync();
        }

        public async Task<List<Clinic_schedule>> GetclinicSchedulebyDoctorIdAsync(String id)
        {

            return await dbContext.Clinic_schedule.Where(x => x.DID == id).ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task BookAppointment(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<usersAndSchedule>> GetAllAppointment()
        {
            var Schedule = await (from schedule in dbContext.Patient_schedule
                                 join patient in dbContext.Users on schedule.PID equals patient.Id
                                 join Doctor in dbContext.Users on schedule.DID equals Doctor.Id
                                 select new usersAndSchedule { schedule = schedule, patient = patient , doctor = Doctor }).ToListAsync();

            
            return Schedule;
        }

        public async Task<IEnumerable<usersAndSchedule>> GetpatientScheduleById(string Id)
        {
            var patientSchedules = await (from schedule in dbContext.Patient_schedule
                                          join patient in dbContext.Users on schedule.PID equals patient.Id
                                          join Doctor in dbContext.Users on schedule.DID equals Doctor.Id
                                          where (schedule.PID == Id)
                                          select new usersAndSchedule { schedule = schedule, patient = patient, doctor = Doctor }).ToListAsync();

            
            return patientSchedules;
        }

        public async Task<IEnumerable<usersAndSchedule>> GetDoctorScheduleById(string Id)
        {
            var DoctorSchedules = await (from schedule in dbContext.Patient_schedule
                                          join patient in dbContext.Users on schedule.PID equals patient.Id
                                          join Doctor in dbContext.Users on schedule.DID equals Doctor.Id
                                          where (schedule.DID == Id)
                                         where (schedule.status == "Approved")
                                         select new usersAndSchedule { schedule = schedule, patient = patient, doctor = Doctor }).ToListAsync();
            
            return DoctorSchedules;
        }

        

        public async Task<IEnumerable<usersAndSchedule>> GetDepartmentScheduleById(int Id)
        {
            var DoctorSchedules = await (from schedule in dbContext.Patient_schedule
                                         join patient in dbContext.Users on schedule.PID equals patient.Id
                                         join Doctor in dbContext.Doctors on schedule.DID equals Doctor.Id
                                         where (Doctor.DepID == Id)
                                         select new usersAndSchedule { schedule = schedule, patient = patient, doctor = Doctor }).ToListAsync();

            return DoctorSchedules;
        }
        public async Task<IEnumerable<usersAndSchedule>> GetclinicScheduleById(int Id)
        {
            var DoctorSchedules = await (from schedule in dbContext.Patient_schedule
                                         join patient in dbContext.Users on schedule.PID equals patient.Id
                                         join Doctor in dbContext.Doctors on schedule.DID equals Doctor.Id
                                         where (schedule.SId == Id)
                                         select new usersAndSchedule { schedule = schedule, patient = patient, doctor = Doctor }).ToListAsync();

            return DoctorSchedules;
        }

        public async Task<IEnumerable<usersAndSchedule>> GetClinicScheduleById(int Id)
        {
            var DoctorSchedules = await (from schedule in dbContext.Patient_schedule
                                         join patient in dbContext.Users on schedule.PID equals patient.Id
                                         join Doctor in dbContext.Doctors on schedule.DID equals Doctor.Id
                                         where (schedule.SId == Id)
                                         select new usersAndSchedule { schedule = schedule, patient = patient, doctor = Doctor }).ToListAsync();

            return DoctorSchedules;
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


        public async Task<Dictionary<int, int>> GetMonthlyPatientScheduleCountAsync()
        {
            var currentYear = DateTime.Now.Year;

            var result = await dbContext.Patient_schedule
                .Where(p => p.date.Year == currentYear)
                .GroupBy(p => p.date.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Month, g => g.Count);

            for (int month = 1; month <= 12; month++)
            {
                if (!result.ContainsKey(month))
                    result[month] = 0;
            }

            return result
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public async Task<Dictionary<int, Dictionary<int, int>>> GetMonthlyPatientCountPerDepartmentAsync(int year)
        {
            var data = await dbContext.Patient_schedule
                .Where(p => p.date.Year == year && p.schedule != null)
                .GroupBy(p => new { p.schedule.DepID, Month = p.date.Month })
                .ToListAsync();

            var result = data
                .GroupBy(g => g.Key.DepID)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(
                        x => x.Key.Month,
                        x => x.Count()
                    )
                );

            return result;
        }
        public bool Delete(TEntity entity)
        {

            if (entity == null)
            {
                return false;
            }

            dbContext.Set<TEntity>().Remove(entity);

            return true;
        }

        public  bool DeleteRange(List<TEntity> entitys)
        {

            if (entitys == null)
            {
                return false;
            }

            dbContext.Set<TEntity>().RemoveRange(entitys);

            return true;
        }


        public async Task<IEnumerable<usersAndSchedule>> GetpatientScheduleByIdAndSId(string PId, int SId)
        {
            var patientSchedules = await (from schedule in dbContext.Patient_schedule
                                          join patient in dbContext.Users on schedule.PID equals patient.Id
                                          join Doctor in dbContext.Users on schedule.DID equals Doctor.Id
                                          where schedule.PID == PId
                                          where schedule.SId == SId
                                          select new usersAndSchedule { schedule = schedule, patient = patient, doctor = Doctor }).ToListAsync();


            return patientSchedules;
        }
    }
}
