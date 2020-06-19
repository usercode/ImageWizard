using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore.Filters.Base
{
    public class FFMpegCoreContext : FilterContext
    {

        public FFMpegCoreContext(ProcessingPipelineContext processingContext)
            : base(processingContext)
        {

        }
    }
}
