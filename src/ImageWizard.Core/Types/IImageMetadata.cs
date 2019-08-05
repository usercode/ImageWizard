using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Types
{
    public interface IImageMetadata
    {
        DateTime? CreatedAt { get; set; }
        string Signature { get; set; }
        string MimeType { get; set; }
        string ImageSource { get; set; }
        double? DPR { get; set; }

        CacheSettings CacheSettings { get; }
    }
}
