using Docnet.Core;
using ImageWizard.Core;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ImageWizard.DocNET
{
    /// <summary>
    /// DocNETPipeline
    /// </summary>
    public class DocNETPipeline : Pipeline<DocNETFilter, DocNETFilterContext>
    {
        public DocNETPipeline(
            IServiceProvider service, 
            ILogger<DocNETPipeline> logger, 
            IEnumerable<PipelineAction<DocNETPipeline>> actions)
            : base(service, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override DocNETFilterContext CreateFilterContext(PipelineContext context)
        {
            return new DocNETFilterContext(context, context.Result.Data);
        }
    }
}
