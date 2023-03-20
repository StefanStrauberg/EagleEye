using FGLogDog.Application;
using FGLogDog.Logging;
using FGLogDog.RabbitMQ.Producer;
using FGLogDog.UDP.Receiver;
using FGLogDog.TCP.Receiver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

Log.Logger = new LoggerConfiguration().WriteTo.Console()
                                      .CreateLogger();
try
{
    IHost host = Host.CreateDefaultBuilder(args)
                     .UseSerilog()
                     .ConfigureServices((hostContext, services) =>
                     {
                         services.AddSingleton<IConfiguration>(hostContext.Configuration);
                         services.AddLoggerServices();
                         services.AddApplicationServices(hostContext.Configuration);
                         services.AddUDPReciverServices(hostContext.Configuration);
                         services.AddTCPReciverServices(hostContext.Configuration);
                         services.AddRabbitMQProducerServices(hostContext.Configuration);
                     })
                     .Build();
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
