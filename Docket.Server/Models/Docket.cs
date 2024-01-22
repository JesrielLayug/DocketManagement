using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docket.Server.Models
{
    [BsonIgnoreExtraElements]
    public class Docket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;
        [BsonElement("rate")]
        public int Rate {  get; set; }
        [BsonElement("body")]
        public string Body { get; set; } = string.Empty;
        [BsonElement("date_created")]
        public DateTime DateCreated { get; set; }
        [BsonElement("date_modified")]
        public DateTime DateModified { get; set; }
        [BsonElement("is_public")]
        public bool IsPublic { get; set; }
        [BsonElement("user_id")]
        public string UserId { get; set; } = string.Empty;
    }
}
