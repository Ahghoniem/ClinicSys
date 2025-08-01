using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs;
using LibraryManagementSystem.DAL.Enums;


namespace LibraryManagementSystem.BLL.ServicesContracts
{

    public interface IUserService
    {
        Task<List<ApplicationUserDto>> GetAllByUserTypeAsync(int userType);
        Task<bool> UpdateAsync(ApplicationUserDto userDto);
        Task<bool> DeleteAsync(string id);
        Task<ApplicationUserDto?> GetByIdAsync(string id);
        Task<bool> UpdateImageUrlAsync(UpdateImageUrlDto model);
        Task<bool> UpdateAddressAsync(UpdateAddressDto model);
        Task<bool> UpdateGenderAsync(UpdateGenderDto model);
        Task<bool> UpdateBirthDateAsync(UpdateBirthDateDto model);
        Task<bool> UpdateBloodTypeAsync(UpdateBloodTypeDto model);
        Task<int> CountAllPatientsAsync();
        Task<Dictionary<string, int>> CountPatientsByDepartmentAsync();
        Task<int> CountPatientsByDepartmentIdAsync(int departmentId);



    }
}
