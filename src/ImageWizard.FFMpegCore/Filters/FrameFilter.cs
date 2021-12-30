using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.FFMpegCore.Filters.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.FFMpegCore.Filters
{
    public class FrameFilter : FFMpegCoreFilter
    {
        [Filter]
        public void GetFrame()
        {
            //var bitmap = FFMpeg.Snapshot(inputPath, new Size(200, 400), TimeSpan.FromMinutes(1));

            MemoryStream mem = new MemoryStream();

            //FFMpegArguments
            //    .FromPipeInput(new StreamPipeSource(Context.Result.Data))
            //    .OutputToPipe(new StreamPipeSink(mem),
            //                options => options
            //                    .ForceFormat("rawvideo")
            //                    .WithVideoCodec(VideoCodec.Png)
            //                    .WithFrameOutputCount(1))
            //    .ProcessSynchronously();
        }
    }
}
