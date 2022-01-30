﻿using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.FFMpegCore.Filters.Base
{
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
}
