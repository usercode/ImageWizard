using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageStorages
{
    public interface IImageStorage
    {
        Task<CachedImage> GetAsync(string key);

        Task<CachedImage> SaveAsync(string key, OriginalImage originalImage, IImageFormat imageFormat, byte[] transformedImageData);
    }
}
