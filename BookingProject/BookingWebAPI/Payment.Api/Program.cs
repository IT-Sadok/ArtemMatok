using Contracts;
using DistributedLocking;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payment.Application.Interfaces.PaymentInterface;
using Payment.Application.Kafka;
using Payment.Application.Services.PaymentService;
using Payment.Application.Validators;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces.PaymentInterface;
using Payment.Infrastructure.Kafka;
using Payment.Infrastructure.Repositories.PaymentRepository;
using SharedInfrastructure;

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

//Services
builder.Services.AddScoped<IPaymentService, PaymentService>();

//Kafka 
builder.Services.Configure<ConsumerSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<IHostedService, UserRegisteredKafkaConsumer>();

//DistributedLock with redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddDistributedLocking();


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
