// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Processing.Builder;

/// <summary>
/// PipelineBuilder
/// </summary>
public class PipelineBuilder : IPipelineBuilder
{
    public PipelineBuilder(IServiceCollection services)
    {
        Services = services;
        MimeTypes = new List<string>();
    }

    /// <summary>
    /// MimeTypes
    /// </summary>
    public IList<string> MimeTypes { get; private set; }

    /// <summary>
    /// ServiceProvider
    /// </summary>
    public IServiceCollection Services { get; private set; }
}
