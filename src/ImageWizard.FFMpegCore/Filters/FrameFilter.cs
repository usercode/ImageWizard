using FFMpegCore;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.FFMpegCore.Filters.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore.Filters
{
    public class FrameFilter : FFMpegCoreFilter
    {
        [Filter]
        public void GetFrame()
        {
            
        }
    }
}
