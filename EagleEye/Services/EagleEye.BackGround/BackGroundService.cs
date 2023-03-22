using EagleEye.BackGround.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EagleEye.BackGround
{
    public static class BackGroundService
    {
        public static IServiceCollection AddBackGroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BackGroundTimer>(
                configuration.GetSection(nameof(BackGroundTimer)));
            services.AddSingleton<IBackGroundTimer>(provider =>
                provider.GetRequiredService<IOptions<BackGroundTimer>>().Value);

            services.AddHostedService<BackGroundTimerService>();
            return services;
        }
    }
}