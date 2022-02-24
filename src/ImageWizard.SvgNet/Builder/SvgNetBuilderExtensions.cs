// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Filters;
using ImageWizard.SvgNet;
using ImageWizard.SvgNet.Builder;
using ImageWizard.SvgNet.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    public static class SvgNetBuilderExtensions
    {
        public static ISvgNetBuilder WithFilter<TFilter>(this ISvgNetBuilder builder) 
            where TFilter : SvgFilter, new()
        {
            builder.Services.AddTransient<TFilter>();
            builder.Services.AddSingleton(new PipelineAction<SvgPipeline>(x => x.AddFilter<TFilter>()));

            return builder;
        }

        public static ISvgNetBuilder WithOptions(this ISvgNetBuilder builder, Action<SvgOptions> action)
        {
            builder.Services.Configure(action);

            return builder;
        }
    }
}
