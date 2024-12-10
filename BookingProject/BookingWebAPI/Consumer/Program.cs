using Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Start consuming....");

var builder = Host.CreateApplicationBuilder();

builder.Services.AddHostedService<EventConsumer>();

builder.Build().Run();