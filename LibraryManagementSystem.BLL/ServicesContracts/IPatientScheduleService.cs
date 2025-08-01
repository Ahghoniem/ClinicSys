using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IPatientScheduleService
    {
        Task<int> BookAppointment(BookAppointmentDTO BookAppointmentDTO);

        Task<IEnumerable<GetAllBookAppointmentDTO>> GetAllBookAppointment();

        Task<IEnumerable<GetAllBookAppointmentDTO>> GetPatientSchedules(string Id);

        Task<IEnumerable<GetAllBookAppointmentDTO>> GetDoctorSchedules(string Id);
        Task<IEnumerable<GetAllBookAppointmentDTO>> GetDepartmentSchedules(int Id);
        public Task<Dictionary<int, int>> GetPatientCountPerMonthAsync();
        Task<IEnumerable<PatientInfoDTO>> GetDoctorPatientInfo(string Id);

        Task<Patient_schedule> AddMedicine(AddMedicineDTO addMedicine);
        Task<Patient_schedule> approvalAppointment(ApprovalDTO Approval);
        public  Task<Dictionary<int, Dictionary<int, int>>> GetPatientCountPerMonthPerDepartmentAsync();
        public  Task<int> DeleteAppointment(int id);
        Task<Patient_schedule> Reschedule(RescheduleDTO Reschedule);

    }
}
