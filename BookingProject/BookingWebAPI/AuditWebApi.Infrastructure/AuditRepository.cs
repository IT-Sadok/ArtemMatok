using AuditWebApi.Application;
using AuditWebApi.Domain.Entities;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Mongo;
using MongoDB.Driver;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Infrastructure
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IMongoCollection<AuditRecord> _collection;
        private readonly ILogger<AuditRepository> _logger;
        public AuditRepository(MongoDbContext context, ILogger<AuditRepository> logger)
        {
            _collection = context.GetCollection<AuditRecord>("AuditLogs");
            _logger = logger;
        }
        public async Task AddAsync(AuditRecord record)
        {
            try
            {
                await _collection.InsertOneAsync(record);   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<Result<AuditRecord>> GetUserByTime(string userId, DateTime timestamp)
        {
            var filter = Builders<AuditRecord>.Filter.And(
                Builders<AuditRecord>.Filter.Eq(x => x.UserId, userId),
                Builders<AuditRecord>.Filter.Lte(x => x.Timestamp, timestamp)
            );
            var sort = Builders<AuditRecord>.Sort.Descending(x => x.Timestamp);


            var result = await _collection.Find(filter)
                .Sort(sort)
                .FirstOrDefaultAsync();

            if (result is null)
            {
                return Result<AuditRecord>.Failure("No audit record found");
            }

            return Result<AuditRecord>.Success(result);
        }
    }
}
