using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryManagementSystem.BLL.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryMAnagementSystem.API.ResponseHandler;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.BLL.Services;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.BLL;

namespace LibraryMAnagementSystem.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class Appointment(IPatientScheduleService PatientScheduleService ,IAdminServices AdminServices) : BaseAPIController
    {
        [HttpPost]
        public async Task<ActionResult<BookAppointmentDTO>> BookAppointment([FromBody] BookAppointmentDTO BookAppointmentDTO)
        {
            var res = await PatientScheduleService.BookAppointment(BookAppointmentDTO);

            if (res == 1)
            {
                var successResponse = new OldApiResponse<BookAppointmentDTO>(201, " successfully Book ", BookAppointmentDTO);
                return Ok(successResponse);
            }
            if (res == -1)
            {
                var errorResponse = new OldApiResponse<object>(400, "id not exist");
                return BadRequest(errorResponse);
            }
            if (res == -2)
            {
                var errorResponse = new OldApiResponse<object>(400, "Time not available");
                return BadRequest(errorResponse);
            }

            var errorResponse1 = new OldApiResponse<object>(400, "Failed to add brand");
            return BadRequest(errorResponse1);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<GetAllBookAppointmentDTO>>> GetAllBookAppointment()
        {
            var res = await PatientScheduleService.GetAllBookAppointment();
            
            if (res.Count() == 0)
            {
                return NotFound("there are no appointments");
            }

            return Ok(res);

        }

        [HttpGet("DoctorPatient{Id}")]
        public async Task<ActionResult<IEnumerable<PatientInfoDTO>>> GetDoctorPatient([FromRoute] string Id)
        {
            var res = await PatientScheduleService.GetDoctorPatientInfo(Id);

            if (res.Count() == 0)
            {
                return NotFound("there are no appointments");
            }

            return Ok(res);

        }

        [HttpGet("GetPatientSchedules{id}")]

        public async Task<ActionResult<IEnumerable<GetAllBookAppointmentDTO>>> GetPatientSchedules([FromRoute] String id)
        {
            var res = await PatientScheduleService.GetPatientSchedules(id);
            
            if (res.Count() == 0)
            {
                return NotFound("there are no appointments");
            }

            return Ok(res);

        }

        [HttpGet("GetDoctorSchedules{Id}")]

        public async Task<ActionResult<IEnumerable<GetAllBookAppointmentDTO>>> GetDoctorSchedules([FromRoute] String Id)
        {
            var res = await PatientScheduleService.GetDoctorSchedules(Id);
            if (res.Count() == 0)
            {
                return NotFound("there are no appointments");
            }
            return Ok(res);
        }

        [HttpPut("AddMedicine")]

        public async Task<ActionResult<Patient_schedule>> AddMedicine([FromBody] AddMedicineDTO addMedicine)
        {
            var res = await PatientScheduleService.AddMedicine(addMedicine);
            var successResponse = new OldApiResponse<AddMedicineDTO>(200, " successfully update ", addMedicine);
            return Ok(successResponse);
        }

        [HttpGet("monthly-count")]
        public async Task<IActionResult> GetMonthlyPatientScheduleCount()
        {
            var result = await PatientScheduleService.GetPatientCountPerMonthAsync();
            return Ok(new
            {
                Year = DateTime.Now.Year,
                MonthlyCounts = result
            });
        }
        [HttpGet("MonthlyCountPerDepartment")]
        public async Task<IActionResult> GetMonthlyCountPerDepartment()
        {
            var result = await PatientScheduleService.GetPatientCountPerMonthPerDepartmentAsync();
            return Ok(new
            {
                Message = "Patient count per month per department retrieved successfully.",
                Data = result
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await PatientScheduleService.DeleteAppointment(id);

            return result switch
            {
                1 => Ok(new ApiResponse<object>(200, "Appointment deleted successfully")),
                -1 => NotFound(new ApiResponse<object>(404, "Appointment not found")),
                _ => BadRequest(new ApiResponse<object>(400, "Failed to delete appointment"))
            };
        }
        [HttpPut("ReSchedule")]

        public async Task<ActionResult<Patient_schedule>> ReSchedule([FromBody] RescheduleDTO Reschedule)
        {
            var res = await PatientScheduleService.Reschedule(Reschedule);



            var successResponse = new OldApiResponse<RescheduleDTO>(200, " successfully update ", Reschedule);
            return Ok(successResponse);




        }
        [HttpPut("ReplaceDoctor")]

        public async Task<ActionResult<Patient_schedule>> ReplaceDoctor([FromBody] ReplaceDoctorDTO replaceDoctorDTO)
        {
            var res = await AdminServices.ReplaceDoctor(replaceDoctorDTO);



            var successResponse = new OldApiResponse<ReplaceDoctorDTO>(200, " successfully update ", replaceDoctorDTO);
            return Ok(successResponse);




        }
    }
}
