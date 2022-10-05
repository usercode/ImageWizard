// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using System;
using System.Threading.Tasks;

namespace ImageWizard.OpenCvSharp.Filters;

/// <summary>
/// FilterContext
/// </summary>
public class OpenCvSharpFilterContext : FilterContext
{
    public OpenCvSharpFilterContext(PipelineContext processingContext)
        : base(processingContext)
    {

    }



    public override void Dispose()
    {

    }

    public override async Task<DataResult> BuildResultAsync()
    {
        //using var haarCascade = new CascadeClassifier(TextPath.HaarCascade);
        //using Mat src = new Mat("lenna.png", ImreadModes.Unchanged);

        //Stream mem = ProcessingContext.StreamPool.GetStream();

        //src.WriteToStream(mem);

        //mem.Seek(0, SeekOrigin.Begin);

        //return new DataResult(mem, MimeTypes.Png);

        throw new Exception();
    }
}
