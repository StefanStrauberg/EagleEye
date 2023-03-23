using FGLogDog.Application;
using FGLogDog.ComponentsOfServer;
using FGLogDog.Logging;
using FGLogDog.ParserFactory;
using FGLogDog.RabbitMQ.Producer;
using FGLogDog.TCP.Receiver;
using FGLogDog.TemporaryBuffer;
using FGLogDog.UDP.Receiver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                                      .WriteTo.File($"FGLogDog-{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.log")
                                      .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                                      .CreateLogger();
try
{
    IHost host = Host.CreateDefaultBuilder(args)
                     .UseSerilog()
                     .ConfigureServices((hostContext, services) =>
                     {
                         services.AddSingleton<IConfiguration>(hostContext.Configuration);
                         services.AddLoggerServices();
                         services.AddApplicationServices();
                         services.AddParserFactoryServices();
                         services.AddTemporaryBufferServices();
                         services.AddComponentsOfServerServices();
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
