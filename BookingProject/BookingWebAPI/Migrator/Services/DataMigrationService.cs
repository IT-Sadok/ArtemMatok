using System.Security.Cryptography;
using System.Text.Json;
using BookingWebApi.Application.Models;
using BookingWebApi.Infrastructure.Data;
using Migrator.Interfaces;
using AutoMapper;
using BookingWebApi.Application.Decorators;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BookingWebApi.Application.Interfaces;

namespace Migrator.Services
{
    public class DataMigrationService : IDataMigrationService
    {
        private const string _dataDirectory = "CompanyFiles";
        private readonly ApplicationDbContext _context;
        private readonly IUserManagerDecorator<AppUser> _userManager;
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;


        public DataMigrationService(ApplicationDbContext context, IUserManagerDecorator<AppUser> userManager, IMapper mapper, IAppUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> MigrateData(string fileName)
        {
            string basePath = AppContext.BaseDirectory; 
            string companyFilePath = Path.Combine(basePath, _dataDirectory, fileName);

            if (!File.Exists(companyFilePath))
            {
                return Result<bool>.Failure("File not found");
            }
            
            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await MigrateFile(companyFilePath);

                if(!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }

                await dbTransaction.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return Result<bool>.Failure(ex.Message);
            }
        }

        private async Task<Result<bool>> MigrateFile(string companyFilePath)
        {
            var hosts = await ReadDataFromFile<List<AppUserMigrationDto>>(companyFilePath);

            if (hosts == null)
            {
                return Result<bool>.Failure("File not found");
            }

            foreach (var item in hosts)
            {
                if (await _userRepository.UserExistsByIdAndCompany(item.Id, item.SourceCompanyId))
                {
                    Console.WriteLine($"User with ExternalId: {item.Id} and SourceCompanyId: {item.SourceCompanyId} already exists. Skipping creation.");
                    continue;
                }

                var newHost = _mapper.Map<AppUser>(item);
                
                var createdUser = await _userManager.CreateAsync(newHost,GenerateDefaultPassword());
                if (!createdUser.Succeeded)
                {
                    var errors = createdUser.Errors.Select(x => x.Description).ToString();
                    return Result<bool>.Failure(errors);
                }
                
                var roleResult = await _userManager.AddToRoleAsync(newHost, UserRoles.Host);
                if (!roleResult.Succeeded)
                {
                    var errorsRole = roleResult.Errors.Select(x => x.Description).ToList();
                    return Result<bool>.Failure(errorsRole);
                }
            }
            return Result<bool>.Success(true);
        }
        
        private async Task<T?> ReadDataFromFile<T>(string filePath)
        {
            var data = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(data);
        }
        
        private string GenerateDefaultPassword()
        {
            //TODO: more complex generating password
            return new string("DefaultPassword1@");
        }
    }
}
