using AuditWebApi.Application;
using AuditWebApi.Domain.Entities;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Mongo;
using MongoDB.Driver;
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
    }
}
