﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing.Results;
using ImageWizard.Utils;

namespace ImageWizard.Processing;

/// <summary>
/// PipelineContext
/// </summary>
public class PipelineContext : IDisposable
{
    public PipelineContext(
        IServiceProvider serviceProvider,
        IStreamPool streamPool,
        DataResult result, 
        ClientHints clientHints, 
        ImageWizardOptions imageWizardOptions,
        IEnumerable<string> acceptMimeTypes,
        IEnumerable<FilterSegment> urlFilters)
    {
        ServiceProvider = serviceProvider;
        StreamPool = streamPool;
        ClientHints = clientHints;
        ImageWizardOptions = imageWizardOptions;
        AcceptMimeTypes = acceptMimeTypes;
        UrlFilters = new Queue<FilterSegment>(urlFilters);

        _dataResult = result;
    }

    /// <summary>
    /// ServiceProvider
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// StreamPooling
    /// </summary>
    public IStreamPool StreamPool { get; }

    private DataResult _dataResult;

    /// <summary>
    /// Result
    /// </summary>
    public DataResult Result 
    {
        get => _dataResult;
        set
        {
            _dataResult.Dispose();

            _dataResult = value;
        }
    }

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
    public Queue<FilterSegment> UrlFilters { get; }

    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Result.Dispose();
        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
