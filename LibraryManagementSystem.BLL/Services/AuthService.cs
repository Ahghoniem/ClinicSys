using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Name)) errors.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(model.Email)) errors.Add("Email is required.");
            if (string.IsNullOrWhiteSpace(model.Password)) errors.Add("Password is required.");
            if (string.IsNullOrWhiteSpace(model.PhoneNumber)) errors.Add("Phone number is required.");

            if (!Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Invalid email format.");

           
            

            if (errors.Any())
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Validation failed",
                    Errors = errors
                };
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Email is already registered.",
                    Errors = new List<string> { "Email is already in use." }
                };
            }

            var user = new Patient
            {
                FullName = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = 4
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));

                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User creation failed",
                    Errors = errors
                };
            }

           

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully",
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                errors.Add("Email and password are required.");
                return new AuthResponseDto { IsSuccess = false, Message = "Validation failed", Errors = errors };
            }

            if (!Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errors.Add("Invalid email format.");
                return new AuthResponseDto { IsSuccess = false, Message = "Validation failed", Errors = errors };
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                errors.Add("Invalid email or password.");
                return new AuthResponseDto { IsSuccess = false, Message = "Login failed", Errors = errors };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                errors.Add("Invalid email or password.");
                return new AuthResponseDto { IsSuccess = false, Message = "Login failed", Errors = errors };
            }

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = token
            };
        }

    }
}
