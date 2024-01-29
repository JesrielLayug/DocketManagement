using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Docket.Server.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string id { get; set; }

        [BsonElement("name")] public string name { get; set; } = string.Empty;

        [BsonElement("gender")] public string gender { get; set; } = string.Empty;

        [BsonElement("age")] public int age { get; set; }

        [BsonElement] public DateTime DateJoined { get; set; }

        [BsonElement("password_hash")] public byte[] PasswordHash { get; set; }

        [BsonElement("password_salt")] public byte[] PasswordSalt {  get; set; }

        [BsonElement("role")] public string Role { get; set; }
    }
}
