// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// IMetadata
    /// </summary>
    public interface IMetadata
    {
        DateTime Created { get; set; }
        string Key { get; set; }
        string Hash { get; set; }
        string MimeType { get; set; }        
        IEnumerable<string> Filters { get; set; }
        string? LoaderSource { get; set; }
        string? LoaderType { get; set; }
        int? Width { get; set; }
        int? Height { get; set; }
        long FileLength { get; set; }
        CacheSettings Cache { get; }
    }
}
