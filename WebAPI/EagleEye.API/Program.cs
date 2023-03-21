using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.EagleEye.Application;
using WebAPI.EagleEye.Infrastructure;
using WebAPI.EagleEye.Application.Middleware;
using WebAPI.EagleEye.RabbitMqMessenger;
using WebAPI.EagleEye.Logging;
using Serilog;
using System;
using EagleEye.TemporaryBuffer;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try
{
    Log.Logger.Information("Application Starting");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddLoggingServices();
    builder.Services.AddApplicationServices();
    builder.Services.AddTemporaryBufferServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddRabbitMQMessengerServices(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("all",
                          builder => builder.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod());
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

    var app = builder.Build();

    app.UseCors("all");
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}