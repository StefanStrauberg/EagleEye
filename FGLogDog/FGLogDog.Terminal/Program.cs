using System;
using System.Threading.Tasks;
using FGLogDog.Application;
using FGLogDog.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Terminal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup DI
            var serviceCollection = new ServiceCollection()
                .AddApplicationServices()
                .BuildServiceProvider();
            
            var mediator = serviceCollection.GetRequiredService<IMediator>();

            var line = await mediator.Send(new GetFGLogQuery());

            Console.WriteLine(line);
        }
    }
}