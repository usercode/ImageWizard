// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageLoadManager
    /// </summary>
    public class TypeManager
    {
        private IDictionary<string, Type> LoaderTypes;

        public TypeManager()
        {
            LoaderTypes = new Dictionary<string, Type>();
        }

        public IEnumerable<string> GetAllKeys()
        {
            return LoaderTypes.Keys;
        }

        public bool ContainsKey(string key)
        {
            return LoaderTypes.ContainsKey(key);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="deliveryType"></param>
        /// <returns></returns>
        public Type Get(string key)
        {
            if (LoaderTypes.TryGetValue(key, out Type? loaderType) == false)
            {
                throw new Exception($"Type was not found: {key}");
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
            if(LoaderTypes.ContainsKey(key) == false)
            {
                LoaderTypes.Add(key, type);
            }
            else
            {
                LoaderTypes[key] = type;
            }
        }
    }
}
