using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageLoaders.Http
{
    /// <summary>
    /// HttpHeaderItem
    /// </summary>
    public class HttpHeaderItem
    {
        public HttpHeaderItem()
        {

        }

        public HttpHeaderItem(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
    }
}
