using ImageWizard.SharedContract.FilterTypes;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore.Builder.Types
{
    public interface IImageDeliveryType : IImageBuildUrl, IImageUrlBuilder
    {
        IImageFilters Image(string deliveryType, string value);
    }
}
