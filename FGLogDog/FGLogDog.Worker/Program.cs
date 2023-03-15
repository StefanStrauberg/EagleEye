using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using FGLogDog.Logging;
using FGLogDog.Application;
using FGLogDog.UDP.Receiver;
using FGLogDog.Console.Producer;
using FGLogDog.RabbitMQ.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using FGLogDog.Application.Contracts;

Log.Logger = new LoggerConfiguration().WriteTo.Console()
                                      .CreateLogger();
try
{
    IHost host = Host.CreateDefaultBuilder(args)
                     .UseSerilog()
                     .ConfigureServices((hostContext, services) =>
                     {
                         IConfiguration configuration = hostContext.Configuration;
                         services.AddSingleton<IConfiguration>(configuration);
                         services.AddLoggerServices();
                         services.AddApplicationServices();
                         services.AddUDPReciverServices();
                         services.AddConsoleProducerServices();
                         services.AddRabbitMQRServices();
                     })
                     .Build();
    var serviceProvider = host.Services;
    using (var service = serviceProvider.GetRequiredService<IServer>())
    {
        await service.StartServer();
    }
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
