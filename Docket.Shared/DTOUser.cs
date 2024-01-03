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
        public string id {  get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public int age { get; set; }
    }
}
