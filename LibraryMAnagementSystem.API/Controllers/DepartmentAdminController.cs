using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryManagementSystem.BLL.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryMAnagementSystem.API.ResponseHandler;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LibraryMAnagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentAdminController(IPatientScheduleService PatientScheduleService) : BaseAPIController
    {
        [HttpPut("approvAppointment")]

        public async Task<ActionResult<Patient_schedule>> AddMedicine([FromBody] ApprovalDTO approval)
        {
            var res = await PatientScheduleService.approvalAppointment(approval);



            var successResponse = new OldApiResponse<ApprovalDTO>(200, " successfully update ", approval);
            return Ok(successResponse);




        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<GetAllBookAppointmentDTO>>> GetDepartmentSchedule(int id)
        {
            var res = await PatientScheduleService.GetDepartmentSchedules(id);
            return Ok(res);




        }
    }
}
