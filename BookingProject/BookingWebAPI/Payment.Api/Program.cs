using Contracts;
using DistributedLocking;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payment.Application.Interfaces.PaymentInterface;
using Payment.Application.Kafka;
using Payment.Application.Services;
using Payment.Application.Services.PaymentService;
using Payment.Application.Validators;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces.OutboxInterface;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Payment.Infrastructure.Kafka;
using Payment.Infrastructure.Repositories.OutboxRepository;
using Payment.Infrastructure.Repositories.PaymentRepository;
using Polly;
using Polly.Retry;
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

//Repositories
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();

//Services
builder.Services.AddScoped<IPaymentService, PaymentService>();

//Kafka 
var test = builder.Configuration.GetSection("KafkaProducer");
builder.Services.Configure<ConsumerSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaProducer"));
builder.Services.AddSingleton<IHostedService, UserRegisteredKafkaConsumer>();
builder.Services.AddSingleton<IPaymentKafkaProducer, PaymentKafkaProducer>();
builder.Services.AddHostedService<OutboxPublisherService>();

//DistributedLock with redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddDistributedLocking();


builder.Services.AddSingleton(provider =>
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
