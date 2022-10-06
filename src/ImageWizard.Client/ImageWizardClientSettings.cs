// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;

namespace ImageWizard;

public class ImageWizardClientSettings : ImageWizardBaseOptions
{
    public ImageWizardClientSettings()
    {
        UseUnsafeUrl = false;
        Enabled = true;
        BaseUrl = ImageWizardDefaults.BasePath;
        Host = string.Empty;
    }

    public bool Enabled { get; set; }

    public bool UseUnsafeUrl { get; set; }

    public string BaseUrl { get; set; }

    public string Host { get; set; }
    
}
