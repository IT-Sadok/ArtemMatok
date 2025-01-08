
using Contracts.Clients;
using Contracts;
using DistributedLocking;

using FluentValidation;
using FluentValidation.AspNetCore;
using Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Payment.Application;
using Payment.Application.EntityDto;
using Payment.Application.Interfaces;
using Payment.Application.Interfaces.PaymentInterface;
using Payment.Application.Kafka;
using Payment.Application.Services;
using Payment.Application.Services.PaymentService;
using Payment.Application.Validators;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Payment.Infrastructure.Kafka;
using Payment.Infrastructure.Repositories;
using Payment.Infrastructure.Repositories.PaymentRepository;
using Polly;
using Polly.Retry;

using Redis;
using SharedInfrastructure;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<BalanceRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
//Db
builder.Services.AddDbContext<PaymentDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

//Repositories
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUserBalanceRepository, UserBalanceRepository>();

//Services
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserBalanceService, UserBalanceService>();
builder.Services.AddScoped<IMonolithClient, MonolithClient>();

//Kafka 
builder.Services.Configure<ConsumerSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<IHostedService, UserRegisteredKafkaConsumer>();


builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddHttpClient("MonolithClient", (provider, client) =>
{
    var apiSettings = provider.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(apiSettings.MonolithUrl);
});

//Redis
//DistributedLock with redis

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});


builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

//builder.Services.AddSingleton<Polly.Registry.ResiliencePipelineProvider<string>>();

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

//Validation
builder.Services.AddValidatorsFromAssemblyContaining<ChangeBalanceRequest>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddDistributedLocking();


builder.Services.AddSingleton<AsyncRetryPolicy>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Program>>();

    return Policy
        .Handle<TimeoutException>()
        .Or<RedisException>()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt =>
            {
                var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(50, 200));
                return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + jitter; 
            },
            onRetry: (exception, timeSpan, retryCount, context) =>
            {
                logger.LogWarning($"Try #{retryCount}. Error: {exception.Message}. Next try after {timeSpan.TotalSeconds} seconds.");
            });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
