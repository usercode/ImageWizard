using ImageWizard.ImageLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    /// <summary>
    /// HttpLoaderManager
    /// </summary>
    class HttpLoaderManager
    {
        public HttpLoaderManager()
        {
            Loaders = new Dictionary<string, IImageLoader>();
        }

        private IDictionary<string, IImageLoader> Loaders { get; }

        /// <summary>
        /// Register
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>()
            where T : IImageLoader, new()
        {
            IImageLoader loader = new T();

            Loaders.Add(loader.DeliveryType, loader);
        }
    }
}
