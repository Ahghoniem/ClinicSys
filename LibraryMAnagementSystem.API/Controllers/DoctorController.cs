using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryMAnagementSystem.API.ResponseHandler;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMAnagementSystem.API.Controllers
{
    public class DoctorController : BaseAPIController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<DoctorDto>>(doctors, "Doctors retrieved successfully", 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new ApiResponse<object>(null, "Doctor not found", 404));

            return Ok(new ApiResponse<DoctorDto>(doctor, "Doctor retrieved successfully", 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _doctorService.CreateAsync(doctorDto);
            if (!success)
                return BadRequest(new ApiResponse<object>(null, "Failed To Add Doctor", 400));

            return Ok(new ApiResponse<AddDoctorDto>(doctorDto, "Doctor created successfully", 200));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _doctorService.UpdateAsync(doctorDto);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Update Doctor", 404));

            return Ok(new ApiResponse<DoctorDto>(doctorDto, "Doctor updated successfully", 200));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _doctorService.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Delete", 404));

            return Ok(new ApiResponse<DoctorDto>(await _doctorService.GetByIdAsync(id), "Doctor deleted successfully", 200));
        }

        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(int departmentId)
        {
            var doctors = await _doctorService.GetDoctorsByDepartmentIdAsync(departmentId);

            if (!doctors.Any())
                return NotFound(new ApiResponse<object>(null, "No doctors found in this department", 404));

            return Ok(new ApiResponse<IEnumerable<DoctorDto>>(doctors, "Doctors retrieved successfully", 200));
        }
        [HttpGet("count/by-department/{departmentId}")]
        public async Task<IActionResult> CountByDepartment(int departmentId)
        {
            var count = await _doctorService.CountByDepartmentIdAsync(departmentId);

            return Ok(new ApiResponse<int>(count, "Doctor count retrieved successfully", 200));
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAll()
        {
            var count = await _doctorService.CountAllAsync();
            return Ok(new ApiResponse<int>(count, "Total doctor count retrieved successfully", 200));
        }



    }
}
