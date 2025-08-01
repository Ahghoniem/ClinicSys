using Microsoft.OpenApi.Models;

namespace LibraryMAnagementSystem.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Define the Swagger document
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic Management System", Version = "v1" });

                c.EnableAnnotations();

                // Define the security scheme for JWT
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer" // The ID must match the key used in AddSecurityDefinition
                    }
                };

                // Add the security scheme to Swagger
                c.AddSecurityDefinition("Bearer", securityScheme);

                // Configure Swagger to handle DateOnly as a string with 'date' format (ISO-8601)
                c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "string",
                    Format = "date", // 'date' format is ISO-8601 (yyyy-MM-dd)
                });

                // Define the security requirement
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // This must match the ID above
                            }
                        },
                        Array.Empty<string>() // Scopes (empty for now)
                    }
                };

                // Add the security requirement to Swagger
                c.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }

    }
}
