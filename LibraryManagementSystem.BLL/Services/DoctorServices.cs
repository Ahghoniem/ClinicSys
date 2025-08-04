using AutoMapper;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Services
{
    public class DoctorServices : IDoctorService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly LibraryDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorServices(UserManager<ApplicationUser> userManager, IMapper mapper, LibraryDbContext context, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateAsync(AddDoctorDto doctorDto)
        {
            var dept = await _unitOfWork.GetRepository<Department>().GetByIdAsync(doctorDto.DepID);
            if (dept == null)
                throw new Exception("User creation failed: Department NotFound");

            // Save image
            var imageName = $"{Guid.NewGuid()}_{doctorDto.ImageUrl.FileName}";
            var uploadPath = Path.Combine("wwwroot", "uploads", "doctors");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await doctorDto.ImageUrl.CopyToAsync(stream);
            }

            var doctor = new Doctor
            {
                UserName = doctorDto.Email,
                Email = doctorDto.Email,
                FullName = doctorDto.FullName,
                DepID = doctorDto.DepID,
                UserType = 3,
                GraduationFaculty = doctorDto.FacultyGraduation,
                ImageUrl = $"uploads/doctors/{imageName}"
            };

            var result = await _userManager.CreateAsync(doctor, $"@{doctorDto.FullName}123@");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }

            return true;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var doctor = await _context.Users.OfType<Doctor>().FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return false;

            _context.Users.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctors = await _context.Users
                .OfType<Doctor>()
                .Include(d => d.Department)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto?> GetByIdAsync(string id)
        {
            var doctor = await _context.Users
                .OfType<Doctor>()
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return null;

            return _mapper.Map<DoctorDto>(doctor);
        }



        public async Task<bool> UpdateAsync(DoctorDto doctorDto)
        {
            var dept = await _unitOfWork.GetRepository<Department>().GetByIdAsync(doctorDto.DepartmentId);
            if (dept == null)
            {
                var errors = string.Join(", ", "Department NotFound");
                throw new Exception($"User creation failed: {errors}");
            }
            var doctor = await _context.Users.OfType<Doctor>().FirstOrDefaultAsync(d => d.Id == doctorDto.Id);
            if (doctor == null) return false;

            doctor.FullName = doctorDto.FullName;
            doctor.Email = doctorDto.Email;
            doctor.UserName = doctorDto.Email;
            doctor.DepID = doctorDto.DepartmentId;

            _context.Users.Update(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            var doctors = await _context.Users
                .OfType<Doctor>()
                .Include(d => d.Department)
                .Where(d => d.DepID == departmentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<int> CountByDepartmentIdAsync(int departmentId)
        {
            return await _context.Users
                .OfType<Doctor>()
                .CountAsync(d => d.DepID == departmentId);
        }
        public async Task<int> CountAllAsync()
        {
            return await _context.Users
                .OfType<Doctor>()
                .CountAsync();
        }



    }
}
