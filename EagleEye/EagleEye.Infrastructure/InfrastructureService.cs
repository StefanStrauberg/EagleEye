using EagleEye.Infrastructure.DatabaseConfig;
using EagleEye.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace WebAPI.EagleEye.Infrastructure
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBConnection>(
                configuration.GetSection(nameof(MongoDBConnection)));
            services.AddSingleton<IMongoDBConnection>(provider =>
                provider.GetRequiredService<IOptions<MongoDBConnection>>().Value);
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            return services;
        }
    }
}