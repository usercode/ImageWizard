using Docnet.Core;
using Docnet.Core.Readers;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.Types;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Services.Types;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
