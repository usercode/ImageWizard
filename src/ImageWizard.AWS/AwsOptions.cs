// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using System;

namespace ImageWizard.AWS;

/// <summary>
/// AwsOptions
/// </summary>
public class AwsOptions : LoaderOptions
{
    public AwsOptions()
    {
        RefreshMode = LoaderRefreshMode.EveryTime;
    }

    /// <summary>
    /// AccessKeyId
    /// </summary>
    public string AccessKeyId { get; set; }

    /// <summary>
    /// SecretAccessKey
    /// </summary>
    public string SecretAccessKey { get; set; }

    /// <summary>
    /// BucketName
    /// </summary>
    public string BucketName { get; set; }
}
