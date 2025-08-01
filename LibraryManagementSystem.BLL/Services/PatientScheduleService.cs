using AutoMapper;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.BLL.Exceptions;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.Enums;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DAL.UOW;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Services
{
    public class PatientScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IMailService _mailService, UserManager<ApplicationUser> _userManager) : IPatientScheduleService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Patient_schedule> AddMedicine(AddMedicineDTO addMedicine)
        {
            if (addMedicine == null)
                throw new ArgumentNullException(nameof(addMedicine));

            var schadule = await unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync(addMedicine.SId);
           
            if (schadule == null)
                throw new ArgumentNullException(nameof(schadule));

            if (schadule.DID != addMedicine.DId)
                throw new ArgumentNullException(nameof(addMedicine));

            var patient = await unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync(addMedicine.SId);

            if (patient.DID != addMedicine.DId)
                throw new NotFoundExceptions($"Patient not found to Doctor Id.", addMedicine.DId);

            if (patient == null)
                throw new NotFoundExceptions($"Patient with ID {addMedicine.SId} not found.", addMedicine.SId);


            patient.PrescriptionMedicine = addMedicine.Medicine;

            bool updated =await unitOfWork.GetPatientRepository<Patient_schedule>().update(patient);

            if (!updated)
                throw new InvalidOperationException("Failed to update the patient.");

            int affectedRows = await unitOfWork.SaveAsync();

            if (affectedRows == 0)
                throw new InvalidOperationException("No records were affected. patient update might have failed.");

            return patient;
        }
        public async Task<Patient_schedule> approvalAppointment(ApprovalDTO Approval)
        {
            if (Approval == null)
                throw new ArgumentNullException(nameof(Approval));

            var patient = await unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync(Approval.SId);
            if (patient == null)
                throw new NotFoundExceptions($"Patient not found.", Approval.SId);

            int scheduleId = (int)patient.SId;
            var clinicSchedule = await unitOfWork.GetPatientRepository<Clinic_schedule>().GetByIdAsync(scheduleId);

            if (clinicSchedule == null)
                throw new NotFoundExceptions($"Schedule with ID {Approval.SId} not found.", Approval.SId);

            // Approve appointment and increment count
            patient.status = "Approved";
            clinicSchedule.count++;

            bool updatedSchedule = await unitOfWork.GetPatientRepository<Clinic_schedule>().update(clinicSchedule);
            if (!updatedSchedule)
                throw new InvalidOperationException("Failed to update the clinic schedule.");

            bool updatedPatient = await unitOfWork.GetPatientRepository<Patient_schedule>().update(patient);
            if (!updatedPatient)
                throw new InvalidOperationException("Failed to update the patient appointment.");

            int affectedRows = await unitOfWork.SaveAsync();
            if (affectedRows == 0)
                throw new InvalidOperationException("No records were affected. Patient update might have failed.");

            // Send email if everything was successful
            if (affectedRows > 0)
            {
                var identityUser = await _userManager.FindByIdAsync(patient.PID);
                if (identityUser != null && !string.IsNullOrEmpty(identityUser.Email))
                {
                    string subject = "Appointment Confirmation";
                    var appointmentDateOnly = clinicSchedule.date.ToString("yyyy-MM-dd");
                    var suggestedArrivalTime = clinicSchedule.date.AddMinutes(-30).ToString("HH:mm");
                    var shift = clinicSchedule.shift == 1 ? "Morning" : "Night";

                    string body = $"Dear {identityUser.FullName},<br>Your appointment has been booked for {appointmentDateOnly} ({clinicSchedule.day}) during the {shift} shift.<br>Please arrive by {suggestedArrivalTime}.";

                    await _mailService.SendEmailAsync(identityUser.Email, subject, body);
                }
            }

            return patient;
        }


        //public async Task<Patient_schedule> approvalAppointment(ApprovalDTO Approval)
        //{
        //    if (Approval == null)
        //        throw new ArgumentNullException(nameof(Approval));

        //    var patient = await unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync(Approval.SId);
        //    int res = (int)patient.SId;

        //    if (patient == null)
        //        throw new NotFoundExceptions($"Patient not found.", Approval.SId);


        //    patient.status = "Approved";

        //        var schedule = await unitOfWork.GetPatientRepository<Clinic_schedule>().GetByIdAsync(res);

        //        if (schedule == null)
        //            throw new NotFoundExceptions($"schedule with ID {Approval.SId} not found.", Approval.SId);

        //        schedule.count++;

        //        bool updated1 = await unitOfWork.GetPatientRepository<Clinic_schedule>().update(schedule);

        //        if (!updated1)
        //            throw new InvalidOperationException("Failed to update the patient.");



        //    bool updated = await unitOfWork.GetPatientRepository<Patient_schedule>().update(patient);


        //    if (!updated)
        //        throw new InvalidOperationException("Failed to update the patient.");

        //    int affectedRows = await unitOfWork.SaveAsync();

        //    if (affectedRows == 0)
        //        throw new InvalidOperationException("No records were affected. patient update might have failed.");



        //    return patient;
        //}

        public async Task<int> BookAppointment(BookAppointmentDTO BookAppointmentDTO)
        {

            if (BookAppointmentDTO == null)
                throw new ArgumentNullException(nameof(BookAppointmentDTO));

            var user = await unitOfWork.GetPatientRepository<ApplicationUser>().GetByIdAsync(BookAppointmentDTO.Id);
            if (user == null)
                return -1;

            var clinicSchedule = await unitOfWork.GetPatientRepository<Clinic_schedule>().GetByIdAsync(BookAppointmentDTO.SId);
            if (clinicSchedule == null)
                return -2;

            DateTime today = DateTime.Today;
            int current = (int)today.DayOfWeek;
            int target = 1;

            if (clinicSchedule.day == "son" || clinicSchedule.day == "Sonday")
            {
                target = (int)DayOfWeek.Sunday;
            }
            if (clinicSchedule.day == "mon" || clinicSchedule.day == "Monday")
            {
                target = (int)DayOfWeek.Monday;
            }
            if (clinicSchedule.day == "tue" || clinicSchedule.day == "Tuesday")
            {
                target = (int)DayOfWeek.Tuesday;
            }
            if (clinicSchedule.day == "wed" || clinicSchedule.day == "Wednesday")
            {
                target = (int)DayOfWeek.Wednesday;
            }
            if (clinicSchedule.day == "thu" || clinicSchedule.day == "Thursday")
            {
                target = (int)DayOfWeek.Thursday;
            }
            if (clinicSchedule.day == "fri" || clinicSchedule.day == "Friday")
            {
                target = (int)DayOfWeek.Friday;
            }
            if (clinicSchedule.day == "sat" || clinicSchedule.day == "Saturday")
            {
                target = (int)DayOfWeek.Saturday;
            }

            var BookAppointmentDTO2 = new Patient_schedule
            {
                date = today.AddDays(target - current),
                day = clinicSchedule.day,
                shift = clinicSchedule.shift,
                PID = BookAppointmentDTO.Id,
                SId = BookAppointmentDTO.SId,
                status = "waiting",
                PrescriptionMedicine = "NULL",
                DID = clinicSchedule.DID
            };

            await unitOfWork.GetPatientRepository<Patient_schedule>().BookAppointment(BookAppointmentDTO2);

            int affectedRows = await unitOfWork.SaveAsync();
            
            return affectedRows;
        }

        public async Task<IEnumerable<GetAllBookAppointmentDTO>> GetAllBookAppointment()
        {
            var Schedule = await unitOfWork.GetPatientRepository<Patient_schedule>().GetAllAppointment();
            var res = Schedule.Select(x => new GetAllBookAppointmentDTO
            {
                Id = x.schedule.ID,
                SId = (int)x.schedule.SId,
                Date = x.schedule.date,
                Day = x.schedule.day,
                Shift = x.schedule.shift,
                DName = x.doctor.FullName,
                DId = x.doctor.Id,
                PId = x.patient.Id,
                PName = x.patient.FullName,
                Medicine = x.schedule.PrescriptionMedicine,
                Status = x.schedule.status
               


            });
           
            return res;
        }

        public async Task<IEnumerable<GetAllBookAppointmentDTO>> GetDoctorSchedules(string Id)
        {

            
            var Schedule = await unitOfWork.GetPatientRepository<Patient_schedule>().GetDoctorScheduleById(Id);
            
            var res = Schedule.Select(x => new GetAllBookAppointmentDTO
            {
                Id = x.schedule.ID,
                SId = (int)x.schedule.SId,
                Date = x.schedule.date,
                Day = x.schedule.day,
                Shift = x.schedule.shift,
                DName = x.doctor.FullName,
                DId = x.doctor.Id,
                PId = x.patient.Id,
                PName = x.patient.FullName,
                Medicine = x.schedule.PrescriptionMedicine,
                Status = x.schedule.status
            });
            

            return res;
        }

        public async Task<IEnumerable<PatientInfoDTO>> GetDoctorPatientInfo(string Id)
        {


            var Schedule = await unitOfWork.GetClinicRepository<DoctorPatient>().GetAllDoctorPatient(Id);

            var patient = new List<DoctorPatient> {};

            var patientId = new List<string> { };

            foreach (var i in Schedule)
            {
                if (!patientId.Contains(i.user.Id))
                {
                    patient.Add(i);
                    patientId.Add(i.user.Id);
                }
            }

            var res = patient.Select(x => new PatientInfoDTO
            {
                  
                
                  FullName = x.user.FullName,
                  Address = x.user.Address,
                  Gender  = x.user.Gender  ,
                  BirthDate  =x.user.BirthDate  ,
                  BloodType = x.user.BloodType ,
                  ImageUrl  = x.user.ImageUrl


            });


            return res;
        }

        public async Task<IEnumerable<GetAllBookAppointmentDTO>> GetDepartmentSchedules(int Id)
        {


            var Schedule = await unitOfWork.GetPatientRepository<Patient_schedule>().GetDepartmentScheduleById(Id);
            var res = Schedule.Select(x => new GetAllBookAppointmentDTO
            {
                Id = x.schedule.ID,
                SId = (int)x.schedule.SId,
                Date = x.schedule.date,
                Day = x.schedule.day,
                Shift = x.schedule.shift,
                DName = x.doctor.FullName,
                DId = x.doctor.Id,
                PId = x.patient.Id,
                PName = x.patient.FullName,
                Medicine = x.schedule.PrescriptionMedicine,
                Status = x.schedule.status
            });
            
            return res;
        }

        public async Task<IEnumerable<GetAllBookAppointmentDTO>> GetPatientSchedules(string Id)
        {

            var Schedule = await unitOfWork.GetPatientRepository<Patient_schedule>().GetpatientScheduleById(Id);
            
            var res = Schedule.Select(x => new GetAllBookAppointmentDTO
            {
                Id = x.schedule.ID,
                SId = (int)x.schedule.SId,
                Date = x.schedule.date,
                Day = x.schedule.day,
                Shift = x.schedule.shift,
                DName = x.doctor.FullName,
                DId = x.doctor.Id,
                PId = x.patient.Id,
                PName = x.patient.FullName,
                Medicine = x.schedule.PrescriptionMedicine,
                Status = x.schedule.status
            });
           
            return res;
        }
        public async Task<Dictionary<int, int>> GetPatientCountPerMonthAsync()
        {
            return await unitOfWork.GetPatientRepository<Patient_schedule>().GetMonthlyPatientScheduleCountAsync();
        }
        public async Task<Dictionary<int, Dictionary<int, int>>> GetPatientCountPerMonthPerDepartmentAsync()
        {
            int currentYear = DateTime.Now.Year;
            return await unitOfWork.GetPatientRepository<Patient_schedule>().GetMonthlyPatientCountPerDepartmentAsync(currentYear);
        }
        public async Task<int> DeleteAppointment(int id)
        {
            var appointment = await unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync(id);
            if (appointment == null)
                return -1;

            unitOfWork.GetPatientRepository<Patient_schedule>().Delete(appointment);
            return await unitOfWork.SaveAsync();
        }

        public async Task<Patient_schedule> Reschedule(RescheduleDTO Reschedule)
        {
            if (Reschedule == null)
                throw new ArgumentNullException(nameof(Reschedule));

            var schadule = await unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync(Reschedule.PSId);

            if (schadule == null)
                throw new ArgumentNullException(nameof(Reschedule));

            var Check = await unitOfWork.GetPatientRepository<Patient_schedule>().GetpatientScheduleByIdAndSId(Reschedule.PId, Reschedule.SId);

            if (Check.Count() != 0)
                throw new Exception("cant book as you are booked in the same time ");



            var ClinicSchadule = await unitOfWork.GetPatientRepository<Clinic_schedule>().GetByIdAsync(Reschedule.SId);

            int res = (int)schadule.SId;
            var ClinicSchaduleBefore = await unitOfWork.GetPatientRepository<Clinic_schedule>().GetByIdAsync(res);

            if (schadule.status == "approved")
            {


                ClinicSchaduleBefore.count--;

                bool updatedCount = await unitOfWork.GetPatientRepository<Clinic_schedule>().update(ClinicSchaduleBefore);

                if (!updatedCount)
                    throw new InvalidOperationException("Failed to update the count.");

                int affectedRows1 = await unitOfWork.SaveAsync();
            }


            schadule.SId = Reschedule.SId;
            schadule.date = ClinicSchadule.date;
            schadule.day = ClinicSchadule.day;
            schadule.shift = ClinicSchadule.shift;
            schadule.DID = ClinicSchadule.DID;
            schadule.status = "waiting";



            bool updated = await unitOfWork.GetPatientRepository<Patient_schedule>().update(schadule);

            if (!updated)
                throw new InvalidOperationException("Failed to update the patient.");

            int affectedRows = await unitOfWork.SaveAsync();

            if (affectedRows == 0)
                throw new InvalidOperationException("No records were affected. patient update might have failed.");

            return schadule;
        }
    }
}
