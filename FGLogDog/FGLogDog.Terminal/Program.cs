using System.IO;
using System.Threading.Tasks;
using FGLogDog.Application;
using FGLogDog.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FGLogDog.Terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup IConfiguration
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            IConfiguration configuration = builder.Build();

            // Setup DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            using(var service = serviceProvider.GetRequiredService<IUdpReceiver>())
            {
                await service.Start();
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure => configure.AddConsole())
                    .AddTransient<UdpReceiver>();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddApplicationServices();
        }
    }
}