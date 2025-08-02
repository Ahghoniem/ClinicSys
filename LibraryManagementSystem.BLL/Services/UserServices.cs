using AutoMapper;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Services
{


    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        private readonly LibraryDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, LibraryDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }


        public async Task<List<ApplicationUserDto>> GetAllByUserTypeAsync(int userType)
        {
            // Step 1: Get all users of the given userType from the database
            var users = await _userManager.Users
                .Where(u => u.UserType == userType)
                .ToListAsync();

            // Step 2: Project each user to ApplicationUserDto in memory
            var result = users.Select(u =>
            {
                var dto = new ApplicationUserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserType = u.UserType,
                    PhoneNumber = u.PhoneNumber
                };

                // If user is a Patient, cast and set additional fields
                if (u is Patient patient)
                {
                    dto.ImageUrl = $"{_configuration["Urls:ApiBaseURl"]}/{patient.ImageUrl}";
                    dto.Address = patient.Address;
                    dto.Gender = patient.Gender;
                    dto.BirthDate = patient.BirthDate;
                    dto.BloodType = patient.BloodType;
                }

                return dto;
            }).ToList();

            return result;
        }

        public async Task<bool> UpdateAsync(ApplicationUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null) return false;
            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.UserName = userDto.Email;
            user.UserType = userDto.UserType;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        public async Task<ApplicationUserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == id)
                .Select(u => new ApplicationUserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserType = u.UserType,
                    PhoneNumber = u.PhoneNumber,
                    // Include Patient-specific properties if the user is a Patient
                    ImageUrl = (u as Patient)!.ImageUrl,
                    Address = (u as Patient)!.Address,
                    Gender = (u as Patient)!.Gender,
                    BirthDate = (u as Patient)!.BirthDate,
                    BloodType = (u as Patient)!.BloodType
                })
                .FirstOrDefaultAsync();

            return user;
        }


        public async Task<bool> UpdateImageUrlAsync(UpdateImageUrlDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id) as Patient;
            if (user == null) return false;

            // Save new image
            var imageName = $"{Guid.NewGuid()}_{model.ImageUrl.FileName}";
            var uploadPath = Path.Combine("wwwroot", "uploads", "patients");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(stream);
            }

            // Optional: Delete old image (if exists)
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                var oldImagePath = Path.Combine("wwwroot", user.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Update the ImageUrl with the relative path
            user.ImageUrl = $"uploads/patients/{imageName}";

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }


        public async Task<bool> UpdateAddressAsync(UpdateAddressDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id) as Patient;
            if (user == null) return false;

            user.Address = model.Address;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateBloodTypeAsync(UpdateBloodTypeDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id) as Patient;
            if (user == null) return false;

            user.BloodType = model.BloodType;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateGenderAsync(UpdateGenderDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id) as Patient;
            if (user == null) return false;

            user.Gender = model.Gender;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateBirthDateAsync(UpdateBirthDateDto model)
        {
            if (model.BirthDate > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("BirthDate cannot be in the future.");

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return false;

            if (user is Patient patient)
            {
                patient.BirthDate = model.BirthDate;
                var result = await _userManager.UpdateAsync(patient);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<int> CountAllPatientsAsync()
        {
            return await _userManager.Users.OfType<Patient>().CountAsync();
        }

        public async Task<Dictionary<string, int>> CountPatientsByDepartmentAsync()
        {
            return await _context.Patient_schedule
                .Where(p => p.schedule != null && p.schedule.Department != null)
                .GroupBy(p => p.schedule.Department.DepName)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    PatientCount = g.Select(p => p.PID).Distinct().Count()
                })
                .ToDictionaryAsync(x => x.DepartmentName ?? "Unknown", x => x.PatientCount);
        }
        public async Task<int> CountPatientsByDepartmentIdAsync(int departmentId)
        {
            return await _context.Patient_schedule
                .Where(p => p.schedule != null && p.schedule.DepID == departmentId)
                .Select(p => p.PID)
                .Distinct()
                .CountAsync();
        }





    }

}
