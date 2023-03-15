using Microsoft.Extensions.DependencyInjection;

namespace RabbitMqMessenger
{
    public static class RabbitMQMessengerService
    {
        public static IServiceCollection AddRabbitMQMessengerServices(this IServiceCollection services)
        {
            services.AddHostedService<RabbitMqListener>();
            return services;
        }
    }
}
