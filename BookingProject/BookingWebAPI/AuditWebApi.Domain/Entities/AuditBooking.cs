using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Domain.Entities
{
    public class AuditBooking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string UserId { get; set; }
        public int ApartamentId { get; set; }
        public DateTime StartDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyName { get; set; }
    }
}
