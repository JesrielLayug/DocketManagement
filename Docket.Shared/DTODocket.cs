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
        public DateTime DateCreated { get; set; }
        public DateTime DateModified {  get; set; }
        public bool IsHidden { get; set; }
        public bool IsPublic { get; set; }
        public string UserId { get; set; }
        public string Username {  get; set; }
    }
}
