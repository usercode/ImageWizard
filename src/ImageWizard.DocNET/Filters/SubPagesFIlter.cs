// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Docnet.Core;
using ImageWizard.Attributes;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing.Results;

namespace ImageWizard.DocNET.Filters;

public partial class SubPagesFilter : DocNETFilter
{
    [Filter]
    public void SubPages(int pageFromIndex, int pageToIndex)
    {
        byte[] buffer = DocLib.Instance.Split(Context.Document.ToByteArray(), pageFromIndex, pageToIndex);

        Context.Result = new DataResult(new MemoryStream(buffer), MimeTypes.Pdf);
    }
}
