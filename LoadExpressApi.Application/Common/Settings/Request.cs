using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.Common.Settings
{
    public class Request
    {
        public required string Data { get; set; }
    }

    public class Response
    {
        public string Data { get; set; }
    }
}
