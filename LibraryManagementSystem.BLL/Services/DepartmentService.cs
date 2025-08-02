using AutoMapper;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.Exceptions;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using LibraryManagementSystem.DAL.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Servicesm
{
    public class DepartmentServices(IUnitOfWork unitOfWork, IMapper mapper) : IDepartmentServices
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            var departments = await unitOfWork.GetRepository<Department>().GetAllAsync();
            return mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id)
        {
            var department = await unitOfWork.GetRepository<Department>().GetByIdAsync(id);
            if (department == null)
                throw new NotFoundExceptions("Department", id);

            return mapper.Map<DepartmentDTO>(department);
        }

        public async Task<int> AddDepartmentAsync(AddDepartmentDTO departmentDto)
        {
            if (departmentDto == null)
                throw new ArgumentNullException(nameof(departmentDto));

            // Save image to wwwroot/uploads/departments
            string imageName = $"{Guid.NewGuid()}_{departmentDto.ImageUrl.FileName}";
            string uploadPath = Path.Combine("wwwroot", "uploads", "departments");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string filePath = Path.Combine(uploadPath, imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await departmentDto.ImageUrl.CopyToAsync(stream);
            }

            // Create Department entity
            var department = new Department
            {
                DepName = departmentDto.Name,
                DepDescription = departmentDto.Dec,
                ImageUrl = $"uploads/departments/{imageName}"  // Save relative path only
            };

            await unitOfWork.GetRepository<Department>().AddAsync(department);
            return await unitOfWork.SaveAsync();
        }


        public async Task<DepartmentDTO> UpdateDepartmentAsync(EditDepartmentDTO departmentDto)
        {
            if (departmentDto == null)
                throw new ArgumentNullException(nameof(departmentDto));

            var department = await unitOfWork.GetRepository<Department>().GetByIdAsync(departmentDto.Id);
            if (department == null)
                throw new NotFoundExceptions("Department", departmentDto.Id);

            mapper.Map(departmentDto, department);

            unitOfWork.GetRepository<Department>().Update(department);
            var result = await unitOfWork.SaveAsync();

            if (result == 0)
                throw new InvalidOperationException("No changes were saved.");

            return mapper.Map<DepartmentDTO>(department);
        }

        public async Task DeleteDepartmentByIdAsync(int id)
        {
            var department = await unitOfWork.GetRepository<Department>().GetByIdAsync(id);
            if (department == null)
                throw new NotFoundExceptions("Department", id);

            var deleted = unitOfWork.GetRepository<Department>().Delete(department);
            if (!deleted)
                throw new InvalidOperationException("Delete failed.");

            var result = await unitOfWork.SaveAsync();
            if (result == 0)
                throw new InvalidOperationException("No changes were saved.");
        }
        public async Task<int> GetDepartmentCountAsync()
        {
            var count = await unitOfWork.GetRepository<Department>().CountAsync();
            return count;
        }

    }


}
