using BookingWebApi.Application.Apartament;
using BookingWebApi.Application.Apartament.Statistics;
using BookingWebApi.Application.Common.Configuration;
using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.User;
using BookingWebApi.Application.User.Decorators;
using BookingWebApi.Application.User.Interfaces;
using BookingWebApi.Application.User.Services;
using BookingWebApi.Application.User.Validator;
using BookingWebApi.Domain.Entities;
using BookingWebApi.Infrastructure.Configuration;
using BookingWebApi.Infrastructure.Data;
using BookingWebApi.Infrastructure.Decorators;
using BookingWebApi.Infrastructure.Kafka;
using BookingWebApi.Middleware;
using Contracts.DTOs;
using FluentValidation;
using Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Redis;
using System.Configuration;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey
        (
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAutoMapper(typeof(UserMapper));

builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
//configurations
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
//Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();    
builder.Services.AddScoped<IApartamentService, ApartamentService>();
builder.Services.AddScoped<IApartamentStatisticService, ApartamentStatisticService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();  

//Repositories
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IApartamentRepository,ApartamentRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

//Decorators
builder.Services.AddScoped<IUserManagerDecorator<AppUser>, UserManagerDecorator<AppUser>>();
builder.Services.AddScoped<ISignInManagerDecorator<AppUser>, SignInManagerDecorator<AppUser>>();

//Kafka
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<IUserChangeKafkaProducer, UserChangeKafkaProducer>();

<<<<<<< HEAD
builder.Services.AddSingleton<IUserRegisteredKafkaProducer, UserRegisteredKafkaProducer>();



=======
//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
>>>>>>> Develop

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true)
);


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();
