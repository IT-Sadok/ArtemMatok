using AuditWebApi.Application.BookingAudit;
using AuditWebApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Mongo;
using MongoDB.Driver;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Infrastructure.Repositories
{
    public class AuditBookingRepository : IAuditBookingRepository
    {
        private readonly IMongoCollection<AuditBooking> _collection;
        private readonly ILogger<AuditBookingRepository> _logger;
        public AuditBookingRepository(MongoDbContext context, ILogger<AuditBookingRepository> logger)
        {
            _collection = context.GetCollection<AuditBooking>("AuditBookingLogs");
            _logger = logger;
        }
        public async Task<Result<bool>> AddAuditBookingAsync(AuditBooking auditBooking)
        {
            try
            {
                await _collection.InsertOneAsync(auditBooking);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> RemoveAuditBookingAsync(string userId, int apartamentId, DateTime startDate)
        {
            var filter = Builders<AuditBooking>.Filter.Where(a =>
                a.UserId == userId &&
                a.ApartamentId == apartamentId &&
                a.StartDate == startDate
            );

            var deleteResult = await _collection.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Audit booking not found for removal.");
        }
    }
}
