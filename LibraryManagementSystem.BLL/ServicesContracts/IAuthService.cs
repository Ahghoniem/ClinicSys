using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto model);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto model);
    }
}
