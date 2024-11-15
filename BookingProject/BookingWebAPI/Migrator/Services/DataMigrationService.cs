using System.Security.Cryptography;
using System.Text.Json;
using BookingWebApi.Infrastructure.Data;
using Migrator.Interfaces;
using AutoMapper;
using BookingWebApi.Application.DTOs.AppUserDTOs;
using BookingWebApi.Domain.Constants;
using BookingWebApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using BookingWebApi.Application.Interfaces;
using Newtonsoft.Json;
using BookingWebApi.Application.Common.Decorators;
using BookingWebApi.Application.Common.Models;

namespace Migrator.Services
{
    public class DataMigrationService : IDataMigrationService
    {
        private const string _dataDirectory = "CompanyFiles";
        private readonly ApplicationDbContext _context;
        private readonly IUserManagerDecorator<AppUser> _userManager;
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DataMigrationService> _logger;

        public DataMigrationService(
            ApplicationDbContext context,
            IUserManagerDecorator<AppUser> userManager,
            IMapper mapper,
            IAppUserRepository userRepository,
            ILogger<DataMigrationService> logger) 
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> MigrateData(string fileName)
        {
            string basePath = AppContext.BaseDirectory;
            string companyFilePath = Path.Combine(basePath, _dataDirectory, fileName);

            if (!File.Exists(companyFilePath))
            {
                _logger.LogError("File not found at path: {FilePath}", companyFilePath);
                return Result<bool>.Failure("File not found");
            }

            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await MigrateFile(companyFilePath);

                if (!result.IsSuccess)
                {
                    _logger.LogError("Migration failed: {ErrorMessage}", result.ErrorMessage);
                    throw new Exception(result.ErrorMessage);
                }

                await dbTransaction.CommitAsync();
                _logger.LogInformation("Data migration completed successfully.");
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                _logger.LogError(ex, "Data migration failed with an exception.");
                return Result<bool>.Failure(ex.Message);
            }
        }

        private async Task<Result<bool>> MigrateFile(string companyFilePath)
        {
            var hosts = DeserializeLargeJson<AppUserMigrationDto>(companyFilePath);

            if (hosts == null)
            {
                _logger.LogWarning("No hosts found in file: {FilePath}", companyFilePath);
                return Result<bool>.Failure("File not found");
            }

            foreach (var item in hosts)
            {
                if (await _userRepository.UserExistsByIdAndCompany(item.Id, item.SourceCompanyId))
                {
                    _logger.LogInformation("User with ExternalId: {ExternalId} and SourceCompanyId: {SourceCompanyId} already exists. Skipping creation.",
                                           item.Id, item.SourceCompanyId);
                    continue;
                }

                var newHost = _mapper.Map<AppUser>(item);
                newHost.Id = Guid.NewGuid().ToString();
                newHost.ExternalId = item.Id;

                var createdUser = await _userManager.CreateAsync(newHost, GenerateDefaultPassword());
                if (!createdUser.Succeeded)
                {
                    var errors = string.Join(", ", createdUser.Errors.Select(x => x.Description));
                    _logger.LogError("Failed to create user with ExternalId: {ExternalId}. Errors: {Errors}", item.Id, errors);
                    return Result<bool>.Failure(errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(newHost, UserRoles.Host);
                if (!roleResult.Succeeded)
                {
                    var errorsRole = string.Join(", ", roleResult.Errors.Select(x => x.Description));
                    _logger.LogError("Failed to assign role to user with ExternalId: {ExternalId}. Errors: {Errors}", item.Id, errorsRole);
                    return Result<bool>.Failure(errorsRole);
                }
            }
            return Result<bool>.Success(true);
        }

        public List<T> DeserializeLargeJson<T>(string filePath)
        {
            var items = new List<T>();
            using (var reader = new StreamReader(filePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.StartObject)
                    {
                        var item = serializer.Deserialize<T>(jsonReader);
                        items.Add(item);
                    }
                }
            }
            return items;
        }

        private string GenerateDefaultPassword()
        {
            return "DefaultPassword1@";
        }
    }
}
