using ImageWizard.ImageLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    /// <summary>
    /// ImageLoadManager
    /// </summary>
    public class ImageLoaderManager
    {
        private IDictionary<string, Type> ImageLoaderTypes;

        public ImageLoaderManager()
        {
            ImageLoaderTypes = new Dictionary<string, Type>();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="deliveryType"></param>
        /// <returns></returns>
        public Type Get(string deliveryType)
        {
            if(ImageLoaderTypes.TryGetValue(deliveryType, out Type loaderType) == false)
            {
                throw new Exception("no loader found: " + deliveryType);
            }

            return loaderType;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <typeparam name="TLoader"></typeparam>
        /// <param name="deliveryType"></param>
        /// <param name="loader"></param>
        public void Register<TLoader>(string deliveryType)
            where TLoader : IImageLoader
        {
            if(ImageLoaderTypes.ContainsKey(deliveryType) == false)
            {
                ImageLoaderTypes.Add(deliveryType, typeof(TLoader));
            }
            else
            {
                ImageLoaderTypes[deliveryType] = typeof(TLoader);
            }
        }
    }
}
