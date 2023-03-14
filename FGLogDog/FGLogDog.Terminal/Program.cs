using FGLogDog.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using FGLogDog.Application;
using FGLogDog.UDP.Receiver;
using FGLogDog.Console.Producer;
using FGLogDog.Logging;

namespace FGLogDog.Terminal
{
    class Program
    {
        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console()
                                                  .CreateLogger();
            try
            {
                // Setup IConfiguration
                var builder = new ConfigurationBuilder();

                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                
                IConfiguration configuration = builder.Build();

                // Setup and configuration DI
                IServiceCollection services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLogging(configure => configure.AddSerilog());
                services.AddApplicationServices();
                services.AddUDPReciverServices();
                services.AddConsoleProducerServices();
                services.AddLoggerServices();

                var serviceProvider = services.BuildServiceProvider();

                // Startup Server
                using (var service = serviceProvider.GetRequiredService<IServer>())
                {
                    await service.StartServer();
                }

                // Delay
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}