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

namespace Migrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var migrationService = host.Services.GetRequiredService<IDataMigrationService>();

            var result = await migrationService.MigrateData("Company.json");

            if (result.IsSuccess)
            {
                Console.WriteLine("Data migration completed successfully.");
            }
            else
            {
                Console.WriteLine($"Data migration failed: {result.ErrorMessage}");
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
                    var path = Path.Combine(basePath, "BookingWebApi", "appsettings.json");

                    config.AddJsonFile(path, optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));


                    services.AddIdentity<AppUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

                    services.AddScoped<IDataMigrationService, DataMigrationService>();
                    services.AddScoped<IUserManagerDecorator<AppUser>, UserManagerDecorator<AppUser>>();
                    services.AddScoped<IAppUserRepository, AppUserRepository>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                });
    }
}
