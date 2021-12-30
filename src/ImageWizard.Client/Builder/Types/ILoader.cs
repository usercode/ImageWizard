using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client.Builder.Types
{
    public interface ILoader : IUrlBuilder
    {
        IFilter LoadData(string loaderType, string loaderSource);
    }
}
