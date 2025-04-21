using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using InstaShare.Infrastructure.Persistence;
using InstaShare.Domain.Repositories;
using InstaShare.Infrastructure.Persistence.Repositories;
using InstaShare.Infrastructure.Services;
using InstaShare.Application.Services;

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

        // Register blob storage service based on configuration
        var useDevStorageStr = configuration["StorageConfig:UseDevStorage"];
        bool useDevStorage = false;
        bool.TryParse(useDevStorageStr, out useDevStorage);
        
        if (useDevStorage)
        {
            services.AddScoped<IBlobStorageService, DevelopmentBlobStorageService>();
        }
        else
        {
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        return services;
    }
}