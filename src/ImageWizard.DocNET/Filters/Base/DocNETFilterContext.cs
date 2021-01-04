using Docnet.Core.Readers;
using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters.Base
{
    public class DocNETFilterContext : FilterContext
    {
        public DocNETFilterContext(ProcessingPipelineContext processingContext, byte[] document)
            : base(processingContext)
        {
            Document = document;
        }

        public byte[] Document { get; }

        public override async Task<ImageResult> BuildResultAsync()
        {
            return new ImageResult(Document, MimeTypes.Pdf);
        }
    }
}
