using ImageWizard.Core.Settings;
using ImageWizard.Core.StreamPooling;
using ImageWizard.Core.Types;
using ImageWizard.Processing.Results;
using ImageWizard.Services.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Processing
{
    /// <summary>
    /// ImageProcessingContext
    /// </summary>
    public class ProcessingPipelineContext
    {
        public ProcessingPipelineContext(
            IStreamPool streamPooling,
            ImageResult result, 
            ClientHints clientHints, 
            ImageWizardOptions imageWizardOptions,
            IEnumerable<string> acceptMimeTypes,
            IEnumerable<string> urlFilters)
        {
            StreamPooling = streamPooling;
            Result = result;
            ClientHints = clientHints;
            ImageWizardOptions = imageWizardOptions;
            AcceptMimeTypes = acceptMimeTypes;
            UrlFilters = new Queue<string>(urlFilters);
        }

        /// <summary>
        /// StreamPooling
        /// </summary>
        public IStreamPool StreamPooling { get; }

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
    }
}
