using AutoMapper;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.UOW;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.DAL.Migrations;

namespace LibraryManagementSystem.BLL.Services
{
    public class AdminServics : IAdminServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly LibraryDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AdminServics(UserManager<ApplicationUser> userManager, IMapper mapper, LibraryDbContext context, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateAsync(AddAdminDto doctorDto)
        {
            var dept = await _unitOfWork.GetRepository<Department>().GetByIdAsync(doctorDto.DepID);
            if (dept == null)
            {
                var errors = string.Join(", ", "Department NotFound");
                throw new Exception($"User creation failed: {errors}");

            }
            var doctor = new Admin
            {
                UserName = doctorDto.Email,
                Email = doctorDto.Email,
                FullName = doctorDto.FullName,
                DepID = doctorDto.DepID,
                UserType = 2,
                PhoneNumber = doctorDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(doctor, $"@{doctorDto.FullName}123@");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }

            return true;
        }

        public async Task<bool> CreateSuperAdminAsync(AddSuperAdminDto userDto)
        {

            var user = new ApplicationUser
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                FullName = userDto.FullName,
                PhoneNumber = userDto.PhoneNumber,
                UserType = 1, 
            };

            var result = await _userManager.CreateAsync(user, $"@{userDto.FullName}123@");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }

            return true;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var doctor = await _context.Users.OfType<Admin>().FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null) return false;

            _context.Users.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AdminDto>> GetAllAsync()
        {
            var doctors = await _context.Users
            .OfType<Admin>()
            .Include(d => d.Department)
            .ToListAsync();

            return _mapper.Map<List<AdminDto>>(doctors);
        }

        public async Task<AdminDto> GetByIdAsync(string id)
        {
            var doctor = await _context.Users
                .OfType<Admin>()
                .FirstOrDefaultAsync(d => d.Id == id);

            return doctor == null ? null : _mapper.Map<AdminDto>(doctor);
        }

        public async Task<bool> ReplaceDoctor(ReplaceDoctorDTO replaceDoctorDTO)
        {
            var doctorSchedule = await _unitOfWork.GetPatientRepository<Patient_schedule>().GetByIdAsync2(replaceDoctorDTO.BId);
            
            var doctorScheduleApproved = new List<Patient_schedule> { };
           
            var doctorScheduleWaiting = new List<Patient_schedule> { };
           
            foreach (var schedule in doctorSchedule)
            {
                if(schedule.status == "Approved") 
                {
                    schedule.DID = replaceDoctorDTO.RId;
                    doctorScheduleApproved.Add(schedule);
                }
                else
                {
                    doctorScheduleWaiting.Add(schedule);
                }
                
            }

            bool delete =  _unitOfWork.GetPatientRepository<Patient_schedule>().DeleteRange(doctorScheduleWaiting);
            
            var ClinicSchedule = await _unitOfWork.GetPatientRepository<Clinic_schedule>().GetclinicSchedulebyDoctorIdAsync(replaceDoctorDTO.BId);
            
            foreach (var schedule in ClinicSchedule)
            {
                schedule.DID = replaceDoctorDTO.RId;
            }

            bool updatedPatientScadule = await _unitOfWork.GetPatientRepository<Patient_schedule>().updateRange(doctorSchedule);
           
            bool updatedClinicScadule = await _unitOfWork.GetPatientRepository<Clinic_schedule>().updateRange(ClinicSchedule);
            
            if (!updatedPatientScadule)
                throw new InvalidOperationException("Failed to update the patientScadule.");

            if (!updatedClinicScadule)
                throw new InvalidOperationException("Failed to update the ClinicScadule.");

            int affectedRows = await _unitOfWork.SaveAsync();


            return true;
        }

        public async Task<bool> UpdateAsync(AdminDto doctorDto)
        {
            var dept = await _unitOfWork.GetRepository<Department>().GetByIdAsync(doctorDto.DepID);
            if (dept == null)
            {
                var errors = string.Join(", ", "Department NotFound");
                throw new Exception($"User creation failed: {errors}");
            }
            var doctor = await _context.Users.OfType<Admin>().FirstOrDefaultAsync(d => d.Id == doctorDto.Id);
            if (doctor == null) return false;

            doctor.FullName = doctorDto.FullName;
            doctor.Email = doctorDto.Email;
            doctor.UserName = doctorDto.Email;
            doctor.DepID = doctorDto.DepID;

            _context.Users.Update(doctor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
