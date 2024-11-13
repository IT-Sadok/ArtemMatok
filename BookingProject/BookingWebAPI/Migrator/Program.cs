using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migrator.Interfaces;
using Migrator.Services;
using BookingWebApi.Infrastructure.Data;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Application.Decorators;
using AutoMapper;
using BookingWebApi.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Migrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var migrationService = host.Services.GetRequiredService<IDataMigrationService>();

            var fileArg = args.FirstOrDefault(arg => arg.StartsWith("--file="));
            string fileName;

            if (fileArg != null)
            {
                fileName = fileArg.Split("=")[1];
            }
            else
            {
                logger.LogError("Error: The '--file=' argument is required. Please specify the file path.");
                return; 
            }

            var result = await migrationService.MigrateData(fileName);

            if (result.IsSuccess)
            {
                logger.LogInformation("Data migration completed successfully.");
            }
            else
            {
                logger.LogError($"Data migration failed: {result.ErrorMessage}");
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                    config.AddJsonFile(path, optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddIdentity<AppUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

                    services.AddLogging();
                    services.AddScoped<IDataMigrationService, DataMigrationService>();
                    services.AddScoped<IUserManagerDecorator<AppUser>, UserManagerDecorator<AppUser>>();
                    services.AddScoped<IAppUserRepository, AppUserRepository>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                });

    }
}
