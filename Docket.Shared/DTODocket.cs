using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docket.Shared
{
    [BsonIgnoreExtraElements]
    public class DTODocket
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id {  get; set; }
        [BsonElement("title")]
        public string Title {  get; set; }
        [BsonElement("body")]
        public string Body {  get; set; }
        [BsonElement("date_created")]
        public DateTime DateCreated { get; set; }
        [BsonElement("date_modified")]
        public DateTime DateModified {  get; set; }
        [BsonElement("is_hidden")]
        public bool IsHidden { get; set; } = false;
        [BsonElement("is_public")]
        public bool IsPublic { get; set; } = false;
        [BsonElement("user_id")]
        [BsonIgnore]
        public string UserId { get; set; }
        [BsonElement("user_name")]
        [BsonIgnore]
        public string Username {  get; set; }
    }
}
