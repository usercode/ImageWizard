using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore.Builder.Types
{
    public interface IImageBuildUrl
    {
        string BuildUrl();
    }
}
