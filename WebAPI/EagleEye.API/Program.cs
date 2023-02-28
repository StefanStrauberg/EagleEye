using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.EagleEye.Application;
using WebAPI.EagleEye.Infrastructure;
using WebAPI.EagleEye.Application.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
