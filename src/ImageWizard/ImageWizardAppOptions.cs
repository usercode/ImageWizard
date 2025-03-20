// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public class ImageWizardAppOptions
{
    public bool UseAnalytics { get; set; } = false;
    public bool UseWebP { get; set; } = false;
    public bool AddMetadata { get; set; } = false;
    public string MetadataCopyright { get; set; } = string.Empty;
}
