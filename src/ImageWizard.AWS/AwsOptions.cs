// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;

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
    public string AccessKeyId { get; set; } = string.Empty;

    /// <summary>
    /// SecretAccessKey
    /// </summary>
    public string SecretAccessKey { get; set; } = string.Empty;

    /// <summary>
    /// BucketName
    /// </summary>
    public string BucketName { get; set; } = string.Empty;
}
