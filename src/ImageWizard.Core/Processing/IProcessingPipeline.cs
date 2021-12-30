using ImageWizard.Filters;
using ImageWizard.Processing.Results;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Processing
{
    public interface IProcessingPipeline
    {
        string[] UsedMimeTypes { get; set; }

        Task<ImageResult> StartAsync(ProcessingPipelineContext context);
    }
}
