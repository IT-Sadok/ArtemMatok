using AuditWebApi.Application;
using AuditWebApi.Infrastructure;
using Contracts.Clients;
using Kafka;
using Microsoft.Extensions.Options;
using Mongo;
using Polly.Retry;
using Polly;
using SharedInfrastructure;
using AuditWebApi.Infrastructure.Repositories;
using AuditWebApi.Application.UserAudit;
using AuditWebApi.Application.BookingAudit;
using BookingWebApi.Application.User.Validator;
using FluentValidation;
using Contracts;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddAutoMapper(typeof(AuditMapper));

builder.Services.AddValidatorsFromAssemblyContaining<AuditBookingCreateDtoValidator>();

//Kafka
builder.Services.Configure<ConsumerSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddHostedService<AuditConsumer>();

//Mongo
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();

//Services
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IMonolithClient,MonolithClient>();
builder.Services.AddScoped<IAuditBookingService, AuditBookingService>();

//Repositories
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditBookingRepository, AuditBookingRepository>();


builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddHttpClient("MonolithClient", (provider, client) =>
{
    var apiSettings = provider.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(apiSettings.MonolithUrl);
});


builder.Services.AddResiliencePipeline("default", x =>
{
    x.AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        Delay = TimeSpan.FromSeconds(2),
        MaxRetryAttempts = 2,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
    })
    .AddTimeout(TimeSpan.FromSeconds(30));
});


builder.Services.AddCustomLogging(builder.Configuration);
builder.Services.AddCustomTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseAuthorization();

app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();
