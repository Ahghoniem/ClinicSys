using AutoMapper;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.BLL.Exceptions;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.Migrations;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DAL.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryManagementSystem.BLL.Services
{
    public class ClinicScheduleServices(IUnitOfWork unitOfWork, IMapper mapper) : IClinicScheduleServices
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        public async Task<int> AddSchedule(AddScheduleDTO Schedule)
        {
            if (Schedule == null)
                throw new ArgumentNullException(nameof(Schedule));

            var user = await unitOfWork.GetPatientRepository<Doctor>().GetByIdAsync(Schedule.DId);
            if (user == null)
                return -1;

            var clinic = new List<Clinic_schedule> { };

            for( int i = 0 ; i < Schedule.day.Length ; i++)
            {
                clinic.Add(new Clinic_schedule
                {
                    date = DateTime.Now,
                    day = Schedule.day[i],
                    shift = Schedule.shift[i],
                    DID = Schedule.DId,
                    DepID = user.DepID,
                });

            }
            

            await unitOfWork.GetClinicRepository<Clinic_schedule>().AddSchedules(clinic);

            int affectedRows = await unitOfWork.SaveAsync();

            return affectedRows;
        }
        public async Task<IEnumerable<object>> GETSchedule()
        {
           var  Schedule = await unitOfWork.GetClinicRepository<Clinic_schedule>().GetAllSchedule();
            var res = Schedule.Select(x => new ScheduleDTO { date = x.schedule.date , 
                                                             day = x.schedule.day,
                                                             shift = x.schedule.shift,
                                                             DoctorName = x.user.FullName,
                                                             DId = x.user.Id,
                                                             DepId = x.schedule.DepID,
                                                             DepartmentName = x.Department.DepName
                                                             

                                                                                        });
            return res;
        }
        public async Task<IEnumerable<object>> GetDoctorSchedule(String Id)
        {
            var Schedule = await unitOfWork.GetClinicRepository<Clinic_schedule>().GetAllDoctorSchedule(Id);


            if (Schedule.Count() == 0)
            {
                throw new NotFoundExceptions($"Doctor schedule with ID {Id} not found.", Id);
            }

            var res = Schedule.Select(x => new GetDoctorScheduleDTO
            {

                SId = x.schedule.ID,
                date = x.schedule.date,
                day = x.schedule.day,
                shift = x.schedule.shift,
                DoctorName = x.user.FullName,
                DId = x.user.Id,
                DepId = x.schedule.DepID,
                DepartmentName = x.Department.DepName



            });
            return res;
        }
        public async Task<bool> updateClinicSchedule(UpdateClinicDoctorDTO UpdateClinicDoctorDTO)
        {
            var clinicSchedules = new List<Clinic_schedule> { };

            var days = new List<String> { };
            var shifts = new List<int> { };
            var SID = new List<int> { };

            for (int i = 0; i < UpdateClinicDoctorDTO.SId.Length; i++)
            {
                var clinicScadule = await unitOfWork.GetClinicRepository<Clinic_schedule>().GetByIdAsync(UpdateClinicDoctorDTO.SId[i]);
                var check = await unitOfWork.GetClinicRepository<Clinic_schedule>().GetAllDoctorSchedule2(clinicScadule.DID, UpdateClinicDoctorDTO.day[i], UpdateClinicDoctorDTO.shift[i]);
                if (check.Count() == 0)
                {
                    days.Add(UpdateClinicDoctorDTO.day[i]);
                    shifts.Add(UpdateClinicDoctorDTO.shift[i]);
                    SID.Add(UpdateClinicDoctorDTO.SId[i]);
                }
            }
            for (int i = 0; i < days.Count(); i++)
            {
                var check = await unitOfWork.GetPatientRepository<Patient_schedule>().GetclinicScheduleById(SID[i]);
                if (check.Count() != 0)
                {
                    throw new Exception("doctor has appnioment in this scadule ");
                }
                else
                {
                    var schedule = await unitOfWork.GetClinicRepository<Clinic_schedule>().GetByIdAsync(SID[i]);
                    schedule.day = days[i];
                    schedule.shift = shifts[i];

                    clinicSchedules.Add(schedule);
                }
            }




            bool update = await unitOfWork.GetClinicRepository<Clinic_schedule>().updateRange(clinicSchedules);

            if (!update)
                throw new InvalidOperationException("Failed to update Clinic Schedule.");

            int affectedRows = await unitOfWork.SaveAsync();

            if (affectedRows == 0)
                throw new InvalidOperationException("No records were affected. patient update might have failed.");



            return true;
        }

        public async Task<IEnumerable<object>> GetDepSchedule(int Id)
        {
            var Schedule = await unitOfWork.GetClinicRepository<Clinic_schedule>().GetAllDepSchedule(Id);

            if (Schedule.Count() == 0)
                throw new NotFoundExceptions($"Department schedule with ID {Id} not found.", Id);


            var res = Schedule.Select(x => new GetDoctorScheduleDTO
            {

                SId = x.schedule.ID,
                date = x.schedule.date,
                day = x.schedule.day,
                shift = x.schedule.shift,
                DoctorName = x.user.FullName,
                DId = x.user.Id,
                DepId = x.schedule.DepID,
                DepartmentName = x.Department.DepName



            });
            return res;
        }
    }
}
