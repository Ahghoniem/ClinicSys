using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(string id);
        public Task<bool> CreateAsync(AddDoctorDto doctorDto);
        Task<bool> UpdateAsync( DoctorDto doctorDto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentIdAsync(int departmentId);
        Task<int> CountByDepartmentIdAsync(int departmentId);
        Task<int> CountAllAsync();
    }
}
