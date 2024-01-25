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
    public class DTODocket
    {
        public string Id {  get; set; }
        public string Title {  get; set; }
        public string Body {  get; set; }
        public List<int> Ratings { get; set; } = new List<int>();

        public double AverageRating
        {
            get
            {
                if (Ratings.Count == 0)
                    return 0;

                return Ratings.Average();
            }
        }
        public bool IsFavorite {  get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified {  get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsPublic { get; set; } = false;
        public string UserId { get; set; }
        public string Username {  get; set; }
    }
}
