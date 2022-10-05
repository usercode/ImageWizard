// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using System.IO;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters.Base;

public class DocNETFilterContext : FilterContext
{
    public DocNETFilterContext(PipelineContext processingContext, Stream document)
        : base(processingContext)
    {
        Document = document;
    }

    public Stream Document { get; }

    public override async Task<DataResult> BuildResultAsync()
    {


        return new DataResult(Document, MimeTypes.Pdf);
    }
}
