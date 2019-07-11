using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageLoaders
{
    /// <summary>
    /// IImageLoader
    /// </summary>
    public interface IImageLoader
    {
        /// <summary>
        /// DeliveryType
        /// </summary>
        string DeliveryType { get; }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        Task<OriginalImage> GetAsync(string requestUri);
    }
}
