using ImageWizard.Core.Settings;
using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageFilters.Base
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public abstract class FilterContext : IDisposable
    {
        public FilterContext(ProcessingPipelineContext context)
        {
            ProcessingContext = context;
        }

        /// <summary>
        /// ProcessingContext
        /// </summary>
        public ProcessingPipelineContext ProcessingContext { get; }

        /// <summary>
        /// Result
        /// </summary>
        public ImageResult? Result { get; set; }

        /// <summary>
        /// NoCache
        /// </summary>
        public bool NoImageCache { get; set; }

        public virtual void Dispose()
        {
            //Result?.Dispose();
        }

        public abstract Task<ImageResult> BuildResultAsync();
    }
}
