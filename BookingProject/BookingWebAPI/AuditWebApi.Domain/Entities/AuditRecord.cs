using Contracts.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Domain.Entities
{

    public class AuditRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; } = null!;
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; }
        public List<UserChange> Changes { get; set; }   
    }
}
