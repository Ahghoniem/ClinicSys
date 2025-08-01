using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryManagementSystem.BLL.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryMAnagementSystem.API.ResponseHandler;
using LibraryManagementSystem.DAL.DTO;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;

namespace LibraryMAnagementSystem.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class Clinic(IClinicScheduleServices ClinicScheduleServices) : BaseAPIController
    {
        [HttpPost]
        public async Task<ActionResult<AddScheduleDTO>> AddSchedule(AddScheduleDTO Schedule)
        {
            var res = await ClinicScheduleServices.AddSchedule(Schedule);

            if (res == Schedule.day.Length)
            {
                var successResponse = new OldApiResponse<AddScheduleDTO>(200, "Schedule added successfully", Schedule);
                return Ok(successResponse);
            }
            if (res == -1)
            {
                var errorResponse1 = new OldApiResponse<object>(400, "doctor id not found");
                return BadRequest(errorResponse1);
            }

            var errorResponse = new OldApiResponse<object>(400, "Failed to add brand");
            return BadRequest(errorResponse);
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedule()
        {
            var res = await ClinicScheduleServices.GETSchedule();


            return Ok(res);

        }
        [HttpGet("DoctorSchedule{Id}")]

        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetDoctorSchedule([FromRoute] String Id)
        {
            var res = await ClinicScheduleServices.GetDoctorSchedule(Id);


            return Ok(res);

        }

        [HttpGet("DepSchedule{Id}")]

        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetDepSchedule([FromRoute] int Id)
        {
            var res = await ClinicScheduleServices.GetDepSchedule(Id);


            return Ok(res);

        }

        [HttpPut("UpdateClinic")]

        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetDepSchedule([FromBody] UpdateClinicDoctorDTO UpdateClinicDoctor)
        {
            var res = await ClinicScheduleServices.updateClinicSchedule(UpdateClinicDoctor);


            return Ok(new ApiResponse<UpdateClinicDoctorDTO>(UpdateClinicDoctor, "clinic schedule updated successfully" , 200));

        }
    }
}
