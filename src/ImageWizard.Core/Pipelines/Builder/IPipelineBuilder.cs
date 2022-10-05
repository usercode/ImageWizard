// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.Core.Processing.Builder;

public interface IPipelineBuilder
{
    IServiceCollection Services { get; }

    IList<string> MimeTypes { get; }
}
