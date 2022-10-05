// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing.Results;

namespace ImageWizard.Processing;

/// <summary>
/// IPipeline
/// </summary>
public interface IPipeline
{
    /// <summary>
    /// StartAsync
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<DataResult> StartAsync(PipelineContext context);
}
