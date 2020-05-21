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
        public CurrentImage(byte[] data, string mimeType)
            : this(data, mimeType, null, null, null)
        {
        }

        public CurrentImage(byte[] data, string mimeType, int? width, int? height, double? dpr)
        {
            Data = data;
            MimeType = mimeType;            
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
