// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public class ImageWizardAppOptions
{
    public ImageWizardAppOptions()
    {
        AddMetadata = false;
        UseWebP = false;
        MetadataCopyright = string.Empty;
    }

    public bool UseAnalytics { get; set; }
    public bool UseWebP { get; set; }
    public bool AddMetadata { get; set; }
    public string MetadataCopyright { get; set; }
}
