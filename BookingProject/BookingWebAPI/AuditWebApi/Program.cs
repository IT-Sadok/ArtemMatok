using AuditWebApi.Application;
using AuditWebApi.Infrastructure;
using AuditWebApi.Infrastructure.Configuration;
using AuditWebApi.Infrastructure.Kafka;
using Mongo;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Kafka
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddHostedService<KafkaConsumer>();

//Mongo
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

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
