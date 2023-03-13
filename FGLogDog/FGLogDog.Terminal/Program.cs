using System.IO;
using System.Threading.Tasks;
using FGLogDog.Application;
using FGLogDog.Application.Contracts;
using FGLogDog.Console.Producer;
using FGLogDog.UDP.Receiver;
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
                serviceCollection.AddSingleton<IConfiguration>(configuration);
                // Add Main Application
                serviceCollection.AddApplicationServices();
                // Inject UDP Reciver
                serviceCollection.AddUDPReciverServices();
                // Inject Console Producer
                serviceCollection.AddConsoleProducerServices();


            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Startup Server
            using(var service = serviceProvider.GetRequiredService<IServer>())
            {
                await service.StartServer();
            }

            // Delay
            System.Console.ReadKey();
        }
    }
}