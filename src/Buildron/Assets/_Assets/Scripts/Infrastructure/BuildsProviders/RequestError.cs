using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Buildron.Infrastructure.BuildsProviders
{
    public class RequestError
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
