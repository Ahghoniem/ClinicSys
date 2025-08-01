using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IClinicScheduleServices
    {
        Task<int> AddSchedule(AddScheduleDTO Schedule);

        Task<IEnumerable<object>> GETSchedule();
        public Task<IEnumerable<object>> GetDepSchedule(int Id);
        public  Task<IEnumerable<object>> GetDoctorSchedule(String Id);

        public Task<bool> updateClinicSchedule(UpdateClinicDoctorDTO UpdateClinicDoctorDTO);

    }
}
