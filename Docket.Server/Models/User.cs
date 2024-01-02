using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docket.Server.Models
{
    [BsonIgnoreExtraElements]
    public class User 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = string.Empty;
        [BsonElement("name")]
        public string name { get; set; } = string.Empty;
        [BsonElement("gender")]
        public string gender { get; set; } = string.Empty;
        [BsonElement("age")]
        public int age { get; set; }
    }
}
