using ImageWizard.FFMpegCore.Filters.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore.Builder
{
    public interface IImageSharpBuilder : IImageWizardBuilder
    {
        IImageSharpBuilder WithOptions(Action<FFMpegOptions> action);

        IImageSharpBuilder WithFilter<TFilter>() where TFilter : FFMpegFilter;


    }
}
