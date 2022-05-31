using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GitHubAPIRequests
{
    public class Issue
    {
        public long id { get; set; }
        public long number { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}
