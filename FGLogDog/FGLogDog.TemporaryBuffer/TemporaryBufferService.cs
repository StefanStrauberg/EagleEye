using FGLogDog.Application.Contracts.TemporaryBuffer;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.TemporaryBuffer
{
    public static class TemporaryBufferService
    {
        /// <summary>
        /// Adding a temporary shared buffer to the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTemporaryBufferServices(this IServiceCollection services)
        {
            services.AddSingleton<IBufferRepository, BufferRepository>();
            return services;
        }
    }
}