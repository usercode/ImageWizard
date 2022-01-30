using ImageWizard.Core.Processing.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class PipelineBuilderExtensions
    {
        /// <summary>
        /// Uses the pipeline for the defined mime types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="mimeTypes"></param>
        /// <returns></returns>
        public static T WithMimeTypes<T>(this T builder, params string[] mimeTypes)
            where T : IPipelineBuilder
        {
            foreach (string mimeType in mimeTypes)
            {
                builder.MimeTypes.Add(mimeType);
            }

            return builder;
        }
    }
}
