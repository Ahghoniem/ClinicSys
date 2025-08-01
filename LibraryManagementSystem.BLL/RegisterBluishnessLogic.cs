using LibraryManagementSystem.BLL.Services;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.BLL.Servicesm;
using LinkDev.Talabat.Core.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.BLL
{
    public static class RegisterBluishnessLogic
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(IProductServices), typeof(ProductServices));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IClinicScheduleServices), typeof(ClinicScheduleServices));
            services.AddScoped(typeof(IPatientScheduleService), typeof(PatientScheduleService));
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IDoctorService, DoctorServices>();
            services.AddScoped<IAdminServices, AdminServics>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDoctorSchedueReportServcies, DoctorSchedueReportServcies>();
            services.AddScoped<IDoctorVistsReport, DoctorVistsReportServices>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<IMailService, MailService>();
  



            return services;
        }
        

    }
}
