using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docket.Server.Models
{
    public class DocketFavorite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {  get; set; }
        [BsonElement("is_favorite")]
        public bool IsFavorite { get; set; }
        [BsonElement("docket_id")]
        public string DocketId { get; set; }
        [BsonElement("user_id")]
        public string UserId {  get; set; }
    }
}
