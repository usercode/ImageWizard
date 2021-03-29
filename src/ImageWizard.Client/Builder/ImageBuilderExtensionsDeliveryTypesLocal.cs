using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class ImageBuilderExtensionsDeliveryTypesLocal
    {
        /// <summary>
        /// Fetch file from wwwroot folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="addFingerprint"></param>
        /// <returns></returns>
        public static IImageFilters FetchLocalFile(this IImageLoaderType imageBuilder, string path, bool addFileVersion = true)
        {
            if (addFileVersion == true)
            {
                path = imageBuilder.FileVersionProvider.AddFileVersionToPath(imageBuilder.HttpContextAccessor.HttpContext.Request.PathBase, path);
            }

            imageBuilder.Image("fetch", path);

            return (IImageFilters)imageBuilder;
        }
    }
}
