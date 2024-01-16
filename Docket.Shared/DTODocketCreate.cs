using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docket.Shared
{
    public class DTODocketCreate
    {
        public string Title {  get; set; }
        public string Body { get; set; }
        public bool IsPublic { get; set; }
    }
}
