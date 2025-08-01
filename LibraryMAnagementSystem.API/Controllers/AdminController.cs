using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryMAnagementSystem.API.ResponseHandler;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.Services;

namespace LibraryMAnagementSystem.API.Controllers
{
    public class AdminController : BaseAPIController
    {
        private readonly IAdminServices _doctorService;

        public AdminController(IAdminServices doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<AdminDto>>(doctors, "Admins retrieved successfully", 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new ApiResponse<object>(null, "Admins not found", 404));

            return Ok(new ApiResponse<AdminDto>(doctor, "Admins retrieved successfully", 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAdminDto doctorDto)
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
                return BadRequest(new ApiResponse<object>(null, "Failed To Add Admins", 400));

            return Ok(new ApiResponse<AddAdminDto>(doctorDto, "Admins created successfully", 200));
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] AddSuperAdminDto userDto)
        {
            try
            {
                // Validate the input
                if (userDto == null)
                {
                    return BadRequest(new ApiResponse<object>(null, "Invalid data", 400));
                }

                var result = await _doctorService.CreateSuperAdminAsync(userDto);

                if (result)
                {
                    return Ok(new ApiResponse<bool>(true, "User created successfully", 200));
                }

                return BadRequest(new ApiResponse<object>(null, "User creation failed", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(null, $"Internal server error: {ex.Message}", 500));
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AdminDto doctorDto)
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
                return NotFound(new ApiResponse<object>(null, "Failed To Update Admins", 404));

            return Ok(new ApiResponse<AdminDto>(doctorDto, "Admins updated successfully", 200));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _doctorService.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Delete", 404));

            return Ok(new ApiResponse<AdminDto>(await _doctorService.GetByIdAsync(id), "Admins deleted successfully", 200));
        }

       
    }
}
