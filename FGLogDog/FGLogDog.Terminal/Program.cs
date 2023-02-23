using System.IO;
using System.Threading.Tasks;
using FGLogDog.Application.Services;
using FGLogDog.Terminal.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            ApplicationServicesConfiguration.ConfigureServices(serviceCollection, configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Startup UDP Server
            using(var service = serviceProvider.GetRequiredService<IUdpServer>())
            {
                await service.ServerStart();
            }
        }
    }
}