using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using InstaShare.Infrastructure.Persistence;
using InstaShare.Domain.Repositories;
using InstaShare.Infrastructure.Persistence.Repositories;

namespace InstaShare.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => {
                options.UseNpgsql(
                        configuration.GetConnectionString("postgresConnection"),
                        npgsqlOptions =>
                        {
                            npgsqlOptions.EnableRetryOnFailure();
                        });

                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();                
            }
        );
    
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}