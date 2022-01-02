using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Processing.Results
{
    /// <summary>
    /// ImageResult
    /// </summary>
    public class ImageResult : DataResult
    {
        public ImageResult(Stream data, string mimeType, int? width, int? height, double? dpr)
            : base(data, mimeType)
        {
            Width = width;
            Height = height;
            DPR = dpr;
        }

        /// <summary>
        /// Width
        /// </summary>
        public int? Width { get; }

        /// <summary>
        /// Height
        /// </summary>
        public int? Height { get; }

        /// <summary>
        /// DPR
        /// </summary>
        public double? DPR { get; }
    }
}
