using ImageWizard.Core.Types;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    /// <summary>
    /// OriginalImage
    /// </summary>
    public class CurrentImage
    {
        public CurrentImage(string mimeType, byte[] data)
            : this(mimeType, data, null, null, null)
        {
        }

        public CurrentImage(string mimeType, byte[] data, int? width, int? height, double? dpr)
        {
            MimeType = mimeType;
            Data = data;
            Width = width;
            Height = height;
            DPR = dpr;
        }

        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get; }

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
