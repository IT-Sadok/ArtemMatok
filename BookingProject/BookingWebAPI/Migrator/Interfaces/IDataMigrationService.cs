using BookingWebApi.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator.Interfaces
{
    public interface IDataMigrationService
    {
        Task<Result<bool>> MigrateData(string fileName);
    }
}
