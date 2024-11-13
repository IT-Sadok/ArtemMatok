using BookingWebApi.Application.Models;

namespace ConsoleMigrator.Interfaces;

public interface IDataMigrationService
{
    Task<Result<bool>> MigrateDate();
}