using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    public class ImageMetadata
    {
        public string SecretKey { get; set; }
        public string MimeType { get; set; }
        public string Url { get; set; }
    }
}
