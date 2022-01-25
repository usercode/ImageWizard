using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class HttpExtensions
    {
        public static ClientHints GetClientHints(this HttpRequest request, double[] allowedDPR)
        {
            ClientHints clientHints = new ClientHints(allowedDPR);

            //check DPR value from request
            string? ch_dpr = request.Headers[ClientHints.DPRHeader].FirstOrDefault();
            string? ch_width = request.Headers[ClientHints.WidthHeader].FirstOrDefault();
            string? ch_viewportWidth = request.Headers[ClientHints.ViewportWidthHeader].FirstOrDefault();

            if (ch_dpr != null)
            {
                clientHints.DPR = double.Parse(ch_dpr, CultureInfo.InvariantCulture);
            }

            if (ch_width != null)
            {
                clientHints.Width = int.Parse(ch_width, CultureInfo.InvariantCulture);
            }

            if (ch_viewportWidth != null)
            {
                clientHints.ViewportWidth = int.Parse(ch_viewportWidth, CultureInfo.InvariantCulture);
            }

            return clientHints;
        }
    }
}
