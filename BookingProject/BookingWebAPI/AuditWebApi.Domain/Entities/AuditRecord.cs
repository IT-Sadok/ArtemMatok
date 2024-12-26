using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace AuditWebApi.Domain.Entities
{
    public class UserChange
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
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
