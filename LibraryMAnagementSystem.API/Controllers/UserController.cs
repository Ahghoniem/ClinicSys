using LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryMAnagementSystem.API.ResponseHandler;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagementSystem.BLL.DTOs.AuthDTOs
{
    public class UserController :BaseAPIController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Patients")]
        public async Task<IActionResult> GetPatients()
        {
            var result = await _userService.GetAllByUserTypeAsync(4);
            return Ok(new ApiResponse<IEnumerable<ApplicationUserDto>>(result, "Patients retrieved successfully", 200));
        }

        [HttpPut]
        public async Task<IActionResult> Update( [FromBody] ApplicationUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _userService.UpdateAsync(userDto);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Update Patients", 404));

            return Ok(new ApiResponse<ApplicationUserDto>(userDto, "Patients updated successfully", 200));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Delete", 404));

            return Ok(new ApiResponse<ApplicationUserDto>(await _userService.GetByIdAsync(id), "Patients deleted successfully", 200));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var doctor = await _userService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(new ApiResponse<object>(null, "Patients not found", 404));

            return Ok(new ApiResponse<ApplicationUserDto>(doctor, "Patients retrieved successfully", 200));
        }

        [HttpPut("UpdateImageUrl")]
        public async Task<IActionResult> UpdateImageUrl([FromBody] UpdateImageUrlDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _userService.UpdateImageUrlAsync(model);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Update Image URL", 404));

            return Ok(new ApiResponse<object>(null, "Image URL updated successfully", 200));
        }

        [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _userService.UpdateAddressAsync(model);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Update Address", 404));

            return Ok(new ApiResponse<object>(null, "Address updated successfully", 200));
        }

        [HttpPut("UpadteBloodType")]
        public async Task<IActionResult> UpdateBloodTypr([FromBody] UpdateBloodTypeDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _userService.UpdateBloodTypeAsync(model);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Update BloodType", 404));

            return Ok(new ApiResponse<object>(null, "BloodType updated successfully", 200));
        }

        [HttpPut("UpdateGender")]
        public async Task<IActionResult> UpdateGender([FromBody] UpdateGenderDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var success = await _userService.UpdateGenderAsync(model);
            if (!success)
                return NotFound(new ApiResponse<object>(null, "Failed To Update Gender", 404));

            return Ok(new ApiResponse<object>(null, "Gender updated successfully", 200));
        }

        [HttpPut("UpdateBirthDate")]
        public async Task<IActionResult> UpdateBirthDate([FromBody] UpdateBirthDateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            try
            {
                var success = await _userService.UpdateBirthDateAsync(model);
                if (!success)
                    return NotFound(new ApiResponse<object>(null, "Failed To Update BirthDate", 404));

                return Ok(new ApiResponse<object>(null, "BirthDate updated successfully", 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message, "Validation Failed", 400));
            }
        }
        [HttpGet("patients/count")]
        public async Task<IActionResult> GetAllPatientCount()
        {
            var count = await _userService.CountAllPatientsAsync();
            return Ok(new ApiResponse<int>(count, "Total patient count", 200));
        }

        [HttpGet("patients/count-by-department")]
        public async Task<IActionResult> GetPatientCountByDepartment()
        {
            var data = await _userService.CountPatientsByDepartmentAsync();
            return Ok(new ApiResponse<Dictionary<string, int>>(data, "Count by department", 200));
        }
        [HttpGet("patients/count-by-department/{departmentId}")]
        public async Task<IActionResult> GetPatientCountByDepartmentId(int departmentId)
        {
            var count = await _userService.CountPatientsByDepartmentIdAsync(departmentId);
            return Ok(new ApiResponse<int>(count, "Patient count for department", 200));
        }




    }
}
    