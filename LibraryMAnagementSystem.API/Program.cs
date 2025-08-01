using LibraryManagementSystem.BLL;
using LibraryManagementSystem.BLL.Hubs;
using LibraryManagementSystem.BLL.Services;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using LibraryManagementSystem.DAL.UOW;
using LibraryMAnagementSystem.API.Errors;
using LibraryMAnagementSystem.API.Extensions;
using LibraryMAnagementSystem.API.MiddleWares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Text;

namespace LibraryMAnagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // your Angular app's origin
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // allow credentials like cookies or tokens
                });
            });
            // Controllers + Validation Response Customization
            builder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = false;
                    options.InvalidModelStateResponseFactory = (action) =>
                    {
                        var errors = action.ModelState.Where(p => p.Value.Errors.Count > 0)
                            .Select(p => new ApiValidationErrorResponse.ValidationError
                            {
                                Field = p.Key,
                                Errors = p.Value!.Errors.Select(e => e.ErrorMessage)
                            });

                        return new BadRequestObjectResult(new ApiValidationErrorResponse
                        {
                            Errors = errors
                        });
                    };
                })
                .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddSignalR();

            // Database Context
            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("StoreContextConnection")));

            //  Identity Setup
            builder.Services.AddTransient(typeof(IDepartmentRepository<>), typeof(DepartmentRepository<>));
            builder.Services.AddTransient(typeof(IPatientSchedule<>), typeof(ClinicSchedule<>));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();

            //  JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", options =>
            {
                var jwtSettings = builder.Configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            //  Services Registration
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddApplicationServices();
            builder.Services.RegisterDataAccessServices();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Custom Global Error Middleware
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            app.UseCors("AllowAngularClient");
            // Swagger (Dev Only)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Status Code Errors (e.g., 404)
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseStaticFiles();
            //app.UseMiddleware<JwtTokenValidationMiddleware>();
            // Auth
            //app.UseAuthentication();
            //app.UseAuthorization();

            //   Map Controllers
            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }





}