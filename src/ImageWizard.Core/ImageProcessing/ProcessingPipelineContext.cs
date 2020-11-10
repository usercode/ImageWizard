using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageProcessing
{
    /// <summary>
    /// ImageProcessingContext
    /// </summary>
    public class ProcessingPipelineContext
    {
        public ProcessingPipelineContext(
            ImageResult result, 
            ClientHints clientHints, 
            ImageWizardOptions imageWizardOptions,
            IEnumerable<string> acceptMimeTypes,
            IEnumerable<string> urlFilters)
        {
            Result = result;
            ClientHints = clientHints;
            ImageWizardOptions = imageWizardOptions;
            AcceptMimeTypes = acceptMimeTypes;
            UrlFilters = new Queue<string>(urlFilters);
        }

        /// <summary>
        /// OriginalImage
        /// </summary>
        public ImageResult Result { get; set; }

        /// <summary>
        /// AcceptMimeTypes
        /// </summary>
        public IEnumerable<string> AcceptMimeTypes { get; set; }

        /// <summary>
        /// ClientHints
        /// </summary>
        public ClientHints ClientHints { get; }

        /// <summary>
        /// ImageWizardOptions
        /// </summary>
        public ImageWizardOptions ImageWizardOptions { get; }

        /// <summary>
        /// UrlFilters
        /// </summary>
        public Queue<string> UrlFilters { get; }

        /// <summary>
        /// DisableCache
        /// </summary>
        public bool DisableCache { get; set; }
    }
}
