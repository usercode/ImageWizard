using Docnet.Core;
using Docnet.Core.Readers;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;

namespace ImageWizard.DocNET.Filters
{
    public class SubPagesFilter : DocNETFilter
    {
        [Filter]
        public void SubPages(int pageFromIndex, int pageToIndex)
        {
            byte[] buffer = DocLib.Instance.Split(Context.Document, pageFromIndex, pageToIndex);

            Context.Result = new ImageResult(buffer, MimeTypes.Pdf);
        }
    }
}
