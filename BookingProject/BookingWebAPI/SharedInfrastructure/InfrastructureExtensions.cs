using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedInfrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Logger = logger;
            return services;
        }

        public static IServiceCollection AddCustomTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddPrometheusExporter();
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddRuntimeInstrumentation();
                    metrics.AddProcessInstrumentation();
                    metrics.AddHttpClientInstrumentation();
                    metrics.AddView("http.server.duration", new ExplicitBucketHistogramConfiguration
                    {
                        Boundaries = new[] { 0.005, 0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1.0 }
                    });
                });

            return services;
        }
    }
}
