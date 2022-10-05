// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using ImageWizard.Attributes;
using ImageWizard.FFMpegCore.Filters.Base;
using ImageWizard.Processing.Results;
using System.IO;

namespace ImageWizard.FFMpegCore.Filters;

public class FrameFilter : FFMpegFilter
{
    [Filter]
    public void Frame()
    {
        //var bitmap = FFMpeg.Snapshot(inputPath, new Size(200, 400), TimeSpan.FromMinutes(1));

        MemoryStream input = Context.ProcessingContext.Result.Data.ToMemoryStream();

        MemoryStream mem = new MemoryStream();

        FFMpegArguments
            .FromPipeInput(new StreamPipeSource(input))
            .OutputToPipe(new StreamPipeSink(mem),
                        options => options
                            .ForceFormat("rawvideo")
                            .WithVideoCodec(VideoCodec.Png)
                            .WithFrameOutputCount(1))
            .ProcessSynchronously();

        mem.Seek(0, SeekOrigin.Begin);

        Context.Result = new DataResult(mem, MimeTypes.Png);
    }
}
