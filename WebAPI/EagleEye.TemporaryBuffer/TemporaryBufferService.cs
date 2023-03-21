using EagleEye.Application.Contracts.TemporaryBuffer;
using EagleEye.TemporaryBuffer.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EagleEye.TemporaryBuffer
{
    public static class TemporaryBufferService
    {
        public static IServiceCollection AddTemporaryBufferServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BufferConfiguration>(
                configuration.GetSection(nameof(BufferConfiguration)));
            services.AddSingleton<IBufferConfiguration>(provider =>
                provider.GetRequiredService<IOptions<BufferConfiguration>>().Value);

            services.AddSingleton<IBufferRepository, BufferRepository>();
            return services;
        }
    }
}