// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;

namespace ImageWizard.FFMpegCore.Filters.Base;

public class FFMpegContext : FilterContext
{

    public FFMpegContext(PipelineContext processingContext)
        : base(processingContext)
    {

    }

    public override Task<DataResult> BuildResultAsync()
    {
        throw new NotImplementedException();
    }
}
