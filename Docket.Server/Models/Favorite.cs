using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docket.Server.Models
{
    public class Favorite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {  get; set; }
        [BsonElement("is_favorite")]
        public bool IsFavorite { get; set; } = false;
        [BsonElement("docket_id")]
        public string DocketId { get; set; }
        [BsonElement("user_id")]
        public string UserId {  get; set; }
    }
}
