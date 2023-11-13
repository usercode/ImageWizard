// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;

namespace ImageWizard;

public class ImageWizardClientSettings : ImageWizardBaseOptions
{
    public bool Enabled { get; set; } = true;

    public bool UseUnsafeUrl { get; set; } = false;

    public string BaseUrl { get; set; } = ImageWizardDefaults.BasePath;

    public string Host { get; set; } = string.Empty;

}
