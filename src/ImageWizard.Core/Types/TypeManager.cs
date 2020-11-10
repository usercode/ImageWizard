using ImageWizard.ImageLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageWizard.Core.ImageLoaders
{
    /// <summary>
    /// ImageLoadManager
    /// </summary>
    public class TypeManager
    {
        private IDictionary<string, Type> ImageLoaderTypes;

        public TypeManager()
        {
            ImageLoaderTypes = new Dictionary<string, Type>();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="deliveryType"></param>
        /// <returns></returns>
        public Type Get(string key)
        {
            if(ImageLoaderTypes.TryGetValue(key, out Type loaderType) == false)
            {
                return null;
            }

            return loaderType;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public void Register<T>(string key)
        {
            Register(key, typeof(T));
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <typeparam name="TLoader"></typeparam>
        /// <param name="deliveryType"></param>
        /// <param name="loader"></param>
        public void Register(string key, Type type)
        {
            if(ImageLoaderTypes.ContainsKey(key) == false)
            {
                ImageLoaderTypes.Add(key, type);
            }
            else
            {
                ImageLoaderTypes[key] = type;
            }
        }
    }
}
