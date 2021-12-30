using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.FFMpegCore.Filters.Base
{
    public class FFMpegCoreContext : FilterContext
    {

        public FFMpegCoreContext(ProcessingPipelineContext processingContext)
            : base(processingContext)
        {

        }

        public override Task<ImageResult> BuildResultAsync()
        {
            throw new NotImplementedException();
        }
    }
}
