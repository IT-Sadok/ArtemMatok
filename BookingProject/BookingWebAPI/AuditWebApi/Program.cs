using AuditWebApi.Application;
using AuditWebApi.Infrastructure;
using Kafka;
using Mongo;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Repositories
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
