using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Types
{
    public interface IImageMetadata
    {
        DateTime? CreatedAt { get; set; }
        string Signature { get; set; }
        string Hash { get; set; }
        string MimeType { get; set; }
        string[] Filters { get; set; }
        string LoaderSource { get; set; }
        string LoaderType { get; set; }
        double? DPR { get; set; }
        bool NoImageCache { get; set; }
        int FileLength { get; set; }
        CacheSettings Cache { get; }
    }
}
