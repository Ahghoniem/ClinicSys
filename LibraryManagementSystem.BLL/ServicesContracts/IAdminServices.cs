using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IAdminServices
    {
        Task<IEnumerable<AdminDto>> GetAllAsync();
        Task<AdminDto> GetByIdAsync(string id);
        public Task<bool> CreateAsync(AddAdminDto adminDto);
        Task<bool> UpdateAsync(AdminDto adminDto);
        Task<bool> DeleteAsync(string id);
        public Task<bool> CreateSuperAdminAsync(AddSuperAdminDto userDto);

        Task<bool> ReplaceDoctor(ReplaceDoctorDTO replaceDoctorDTO);


    }
}
