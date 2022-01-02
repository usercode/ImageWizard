using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Processing
{
    public interface IProcessingPipeline
    {
        string[] UsedMimeTypes { get; set; }

        Task<DataResult> StartAsync(ProcessingPipelineContext context);
    }
}
