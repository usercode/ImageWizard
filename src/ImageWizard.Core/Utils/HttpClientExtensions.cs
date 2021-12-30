using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class HttpClientExtensions
    {
        public static void AddUserAgentHeader(this HttpRequestMessage request)
        {
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("ImageWizard", "3.0"));
        }
    }
}
