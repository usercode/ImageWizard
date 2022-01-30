using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.OpenCvSharp.Filters
{
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
}
