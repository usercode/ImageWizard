using ImageWizard.Core.Types;
using ImageWizard.ImageLoaders;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders
{
    public abstract class ImageLoaderBase : IImageLoader
    {
        public ImageLoaderBase()
        {
            RefreshMode = ImageLoaderRefreshMode.None;
        }

        public virtual ImageLoaderRefreshMode RefreshMode { get; }

        public abstract Task<OriginalImage> GetAsync(string source, ICachedImage existingCachedImage);
    }
}
