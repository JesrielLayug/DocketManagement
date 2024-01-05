using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Docket.Server.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("name")]
        public string name { get; set; } = string.Empty;

        [BsonElement("gender")]
        public string gender { get; set; } = string.Empty;

        [BsonElement("age")]
        public int age { get; set; }

        [BsonElement("password_hash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("password_salt")]
        public byte[] PasswordSalt {  get; set; }
    }
}
