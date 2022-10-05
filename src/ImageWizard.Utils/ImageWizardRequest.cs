// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Http;

namespace ImageWizard.Utils;

public class ImageWizardRequest
{
    public ImageWizardRequest(ImageWizardUrl url, HostString host)
    {
        Url = url;
        Host = host;
    }

    /// <summary>
    /// Url
    /// </summary>
    public ImageWizardUrl Url { get; }

    /// <summary>
    /// Host
    /// </summary>
    public HostString Host { get; }
}
