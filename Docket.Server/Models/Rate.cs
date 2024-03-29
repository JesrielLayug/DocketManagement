﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Docket.Server.Models
{
    public class Rate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("rating")]
        public int Rating { get; set; }
        [BsonElement("docket_id")]
        public string DocketId { get; set; }
        [BsonElement("user_id")]
        public string UserId {  get; set; }
    }
}
