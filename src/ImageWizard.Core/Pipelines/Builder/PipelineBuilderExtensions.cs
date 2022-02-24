// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using ImageWizard.Processing;
using Microsoft.Extensions.DependencyInjection;
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
            builder.MimeTypes.Clear();

            foreach (string mimeType in mimeTypes)
            {
                builder.MimeTypes.Add(mimeType);
            }

            return builder;
        }

        ///// <summary>
        ///// Executes custom action before the pipeline is started.
        ///// </summary>
        ///// <param name="builder"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public static T WithPreProcessing2<T>(this T builder, Action<FilterContext> action)
        //    where T : IPipelineBuilder
        //{
        //    builder.Services.AddSingleton(action);

        //    return builder;
        //}
    }
}
