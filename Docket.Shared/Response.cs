using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Docket.Shared
{
    public class Response
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string token {  get; set; }
    }
}
