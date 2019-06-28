using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    public class OriginalImage
    {
        public OriginalImage(string url, string mimeType, byte[] data)
        {
            Url = url;
            MimeType = mimeType;
            Data = data;
        }

        public string Url { get; set; }
        public string MimeType { get; }
        public byte[] Data { get; }
    }
}
