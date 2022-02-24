// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha;
using Piranha.Extend.Blocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Piranha
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddImageWizardModule(this IServiceCollection services)
        {
            App.Modules.Register<PiranhaModule>();

            return services;
        }

        public static IApplicationBuilder UseImageWizardModule(this IApplicationBuilder builder)
        {
            //// Manager resources
            //App.Modules.Manager().Scripts
            //   .Add("~/manager/simplemodule/js/header-block.js");

            //// Add the embedded resources
            //builder.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new EmbeddedFileProvider(typeof(StartupExtensions).Assembly,
            //        "ImageWizard.Piranha.assets.dist"),
            //    RequestPath = "/manager/imagewizard"
            //});

            return builder;
        }
    }
}
