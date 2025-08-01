using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IDepartmentServices
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDTO> GetDepartmentByIdAsync(int id);
        Task<int> AddDepartmentAsync(AddDepartmentDTO departmentDto);
        Task<DepartmentDTO> UpdateDepartmentAsync(EditDepartmentDTO departmentDto);
        Task DeleteDepartmentByIdAsync(int id);
        Task<int> GetDepartmentCountAsync();
    }


}
