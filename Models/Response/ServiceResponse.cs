using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Models.Response
{
   public class ServiceResponse
    {
        public HttpStatusCode status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
