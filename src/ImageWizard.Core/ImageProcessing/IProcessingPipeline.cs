using ImageWizard.Core.ImageProcessing;
using ImageWizard.Filters;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageFilters.Base
{
    public interface IProcessingPipeline
    {
        IEnumerable<string> MimeType { get; }

        Task StartAsync(ProcessingPipelineContext context);
    }
}
