// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Docnet.Core;
using Docnet.Core.Readers;
using ImageWizard.Attributes;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageWizard.DocNET.Filters;

public class SubPagesFilter : DocNETFilter
{
    [Filter]
    public void SubPages(int pageFromIndex, int pageToIndex)
    {
        byte[] buffer = DocLib.Instance.Split(Context.Document.ToByteArray(), pageFromIndex, pageToIndex);

        Context.Result = new DataResult(new MemoryStream(buffer), MimeTypes.Pdf);
    }
}
