using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docket.Shared
{
    public class DTODocketWithRate
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsPublic { get; set; } = false;
        public int Rates { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
