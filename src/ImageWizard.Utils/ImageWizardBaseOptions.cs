// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Utils;

public abstract class ImageWizardBaseOptions
{
    public ImageWizardBaseOptions()
    {
        Key = Array.Empty<byte>();
    }

    /// <summary>
    /// Key
    /// </summary>
    public byte[] Key { get; set; }
}
