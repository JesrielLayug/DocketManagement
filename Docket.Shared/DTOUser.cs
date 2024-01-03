using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docket.Shared
{
    public class DTOUser
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
    }
}
