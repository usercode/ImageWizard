using Docnet.Core;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using ImageWizard.Core;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Filters;
using ImageWizard.Processing;
using ImageWizard.Services.Types;
using ImageWizard.Utils.FilterTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.DocNET
{
    /// <summary>
    /// DocNETPipeline
    /// </summary>
    public class DocNETPipeline : ProcessingPipeline<DocNETFilter>
    {
        public DocNETPipeline(
            IServiceProvider service, 
            ILogger<DocNETPipeline> logger, 
            IEnumerable<PipelineAction<DocNETPipeline>> actions)
            : base(service, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override IFilterAction CreateFilterAction<TFilter>(Regex regex, MethodInfo methodInfo)
        {
            return new DocNETFilterAction<TFilter>(ServiceProvider, regex, methodInfo);
        }

        protected override FilterContext CreateFilterContext(ProcessingPipelineContext context)
        {
            return new DocNETFilterContext(context, context.Result.Data);
        }
    }
}
