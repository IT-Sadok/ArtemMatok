using Microsoft.Extensions.DependencyInjection;
using Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLocking
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDistributedLocking(this IServiceCollection services)
        {
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            services.AddScoped<IDistributedLockService, DistributedLockService>();

            return services;
        }
    }
}
