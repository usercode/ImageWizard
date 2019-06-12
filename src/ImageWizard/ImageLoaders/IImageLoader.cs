using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageLoaders
{
    public interface IImageLoader
    {
        Task<OriginalImage> GetAsync(string requestUri);
    }
}
