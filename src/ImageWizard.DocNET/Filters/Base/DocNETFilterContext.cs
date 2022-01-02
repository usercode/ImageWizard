using Docnet.Core.Readers;
using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters.Base
{
    public class DocNETFilterContext : FilterContext
    {
        public DocNETFilterContext(ProcessingPipelineContext processingContext, Stream document)
            : base(processingContext)
        {
            Document = document;
        }

        public Stream Document { get; }

        public override async Task<DataResult> BuildResultAsync()
        {


            return new DataResult(Document, MimeTypes.Pdf);
        }
    }
}
