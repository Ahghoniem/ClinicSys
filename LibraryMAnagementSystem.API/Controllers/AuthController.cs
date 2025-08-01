using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryMAnagementSystem.API.Errors;
using LibraryMAnagementSystem.API.ResponseHandler;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMAnagementSystem.API.Controllers
{

    public class AuthController : BaseAPIController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>(errors, "Validation Failed", 400));
            }

            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<string>(result.Errors?.ToList() ?? new(), result.Message, 400));
            }

            return Ok(new ApiResponse<string>(result.Token!, "Registration successful", 200));
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var result = await _authService.LoginAsync(model);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<string>(result.Errors ?? new List<string> { result.Message! }, result.Message!, 400));

            return Ok(new ApiResponse<string>(result.Token!, "Login successful", 200));
        }


    }
}