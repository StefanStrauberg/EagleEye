using FGLogDog.Application.Contracts.TemporaryBuffer;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.TemporaryBuffer
{
    public static class TemporaryBufferService
    {
        public static IServiceCollection AddTemporaryBufferServices(this IServiceCollection services)
        {
            services.AddSingleton<IBufferRepository, BufferRepository>();
            return services;
        }
    }
}