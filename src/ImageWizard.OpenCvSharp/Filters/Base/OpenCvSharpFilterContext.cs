using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using ImageWizard.Utils.FilterTypes;
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
        public OpenCvSharpFilterContext(ProcessingPipelineContext processingContext)
            : base(processingContext)
        {

            NoImageCache = false;
        }



        public override void Dispose()
        {

        }

        public override async Task<ImageResult> BuildResultAsync()
        {


            //update some metadata
            //ProcessingContext.DisableCache = NoImageCache;

            return null;
        }
    }
}
