using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore.Builder.Types
{
    public interface IImageSelector
    {
        IImageFilters Fetch(string url);

        IImageFilters Upload(string path);

        IImageFilters Youtube(string id);
    }
}
