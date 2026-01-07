using Services.Abstraction;
using Services.Implementation;
using Shared.IdentityDtos;

namespace E_Commerce_DotNet.Extensions
{
    public static class CoreServicesExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            services.AddAutoMapper(cfg =>
            {
                //cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
                cfg.AddMaps(typeof(ServiceManager).Assembly);
            });

            return services;
        }
    }
}
