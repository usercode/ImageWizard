using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Processing.Builder
{
    public interface IPipelineBuilder
    {
        IServiceCollection Services { get; }

        IList<string> MimeTypes { get; }
    }
}
