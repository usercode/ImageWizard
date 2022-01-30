using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Processing
{
    /// <summary>
    /// IPipeline
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// StartAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<DataResult> StartAsync(PipelineContext context);
    }
}
