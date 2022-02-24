// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.FFMpegCore;
using ImageWizard.FFMpegCore.Builder;
using ImageWizard.FFMpegCore.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class FFMpegBuilderExtensions
    {
        public static IFFMpegBuilder WithFilter<TFilter>(this IFFMpegBuilder builder) 
            where TFilter : FFMpegFilter
        {
            builder.Services.AddTransient<TFilter>();
            builder.Services.AddSingleton(new PipelineAction<FFMpegPipeline>(x => x.AddFilter<TFilter>()));

            return builder;
        }

        public static IFFMpegBuilder WithOptions(this IFFMpegBuilder builder, Action<FFMpegOptions> action)
        {
            builder.Services.Configure(action);

            return builder;
        }
    }
}
