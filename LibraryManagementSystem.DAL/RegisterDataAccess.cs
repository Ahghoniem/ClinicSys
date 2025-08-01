using LibraryManagementSystem.DAL.DbContext;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DAL.RepositoriesContracts;
using LibraryManagementSystem.DAL.UOW;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL
{
    public static class RegisterDataAccess
    {

        public static IServiceCollection RegisterDataAccessServices(this IServiceCollection services)
        {
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();

            return services;
        }

    }
}
