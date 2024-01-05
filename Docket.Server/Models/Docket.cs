using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docket.Server.Models
{
    public class Docket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("title")]
        public string title { get; set; } = string.Empty;
        [BsonElement("body")]
        public string body { get; set; } = string.Empty;
        [BsonElement("date_created")]
        public DateTime DateCreated { get; set; }
        [BsonElement("date_modified")]
        public DateTime DateModified { get; set; }
        [BsonElement("isHidden")]
        public bool IsHidden { get; set; }
        [BsonElement("color")]
        public string Color { get; set; } = string.Empty;
        [BsonElement("user_id")]
        public string UserId { get; set; } = string.Empty;
    }
}
