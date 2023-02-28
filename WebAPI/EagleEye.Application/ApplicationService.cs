using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.EagleEye.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}