using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebAPI.EagleEye.RabbitMqMessenger.RabbitMQConfig;

namespace WebAPI.EagleEye.RabbitMqMessenger
{
    public static class RabbitMQMessengerService
    {
        public static IServiceCollection AddRabbitMQMessengerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessengerConnection>(
                configuration.GetSection(nameof(MessengerConnection)));
            services.AddSingleton<IMessengerConnection>(provider =>
                provider.GetRequiredService<IOptions<MessengerConnection>>().Value);
            services.AddHostedService<RabbitMqListener>();
            return services;
        }
    }
}
