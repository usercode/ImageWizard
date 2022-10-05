// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using System;
using System.Threading.Tasks;

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
