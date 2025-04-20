using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InstaShare.Application
{

  public static class ConfigureService
  {
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
	  services.AddMediatR(
		  cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

	  return services;
	}
  }
}

