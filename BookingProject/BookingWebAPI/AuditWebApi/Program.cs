using AuditWebApi;
using AuditWebApi.Application;
using AuditWebApi.Infrastructure;
using Contracts.Clients;
using Kafka;
using Microsoft.Extensions.Options;
using Mongo;
using Polly.Retry;
using Polly;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddAutoMapper(typeof(AuditMapper));

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

//Repositories
builder.Services.AddScoped<IAuditRepository, AuditRepository>();


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


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Logger = logger;
builder.Host.UseSerilog();


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
