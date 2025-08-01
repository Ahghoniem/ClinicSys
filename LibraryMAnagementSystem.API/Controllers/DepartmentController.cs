
using Microsoft.AspNetCore.Mvc;

using LibraryManagementSystem.BLL.DTOs;
using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryMAnagementSystem.API.ResponseHandler;
using LibraryManagementSystem.DAL.Entities;
using LibraryMAnagementSystem.API.Filters;

namespace LibraryManagementSystem.API.Controllers
{

    public class DepartmentController : BaseAPIController
    {
        private readonly IDepartmentServices _departmentService;

        public DepartmentController(IDepartmentServices departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(new ApiResponse<object>(departments, "Success", 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            return Ok(new ApiResponse<object>(department, "Success", 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDepartmentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(dto, "Failed", 400));

            var result = await _departmentService.AddDepartmentAsync(dto);
            return Ok(new ApiResponse<object>(dto, "Added Successfully", 200));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EditDepartmentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(dto, "Failed", 400));

            var result = await _departmentService.UpdateDepartmentAsync(dto);
            return Ok(new ApiResponse<object>(result, "Updated Successfully", 200));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _departmentService.GetDepartmentByIdAsync(id);
            await _departmentService.DeleteDepartmentByIdAsync(id);
            return Ok(new ApiResponse<object>(obj, "Deleted Successfully", 200));
        }
        [RoleBasedAuthorization(4)]
        [HttpGet("count")]
        public async Task<IActionResult> GetDepartmentCount()
        {
            var count = await _departmentService.GetDepartmentCountAsync();
            return Ok(new ApiResponse<int>(count, "Total number of departments", 200));
        }

    }
}

