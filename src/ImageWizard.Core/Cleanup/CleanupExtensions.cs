// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Cleanup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public static class CleanupExtensions
{
    public static IImageWizardBuilder AddCleanupService(this IImageWizardBuilder builder, Action<CleanupOptions> options)
    {
        builder.Services.Configure(options);
        builder.Services.AddHostedService<CleanupBackgroundService>();

        return builder;
    }

    public static CleanupOptions Created(this CleanupOptions options, TimeSpan duration)
    {
        options.Reasons.Add(new Created(duration));

        return options;
    }

    public static CleanupOptions LastAccess(this CleanupOptions options, TimeSpan duration)
    {
        options.Reasons.Add(new LastAccess(duration));

        return options;
    }
}
