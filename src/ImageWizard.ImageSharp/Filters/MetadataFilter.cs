// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public partial class MetadataFilter : ImageSharpFilter
{
    [Filter]
    public void RemoveExif()
    {
        Context.Image.Metadata.ExifProfile = null;
    }

    [Filter]
    public void RemoveIcc()
    {
        Context.Image.Metadata.IccProfile = null;
    }

    [Filter]
    public void RemoveIptc()
    {
        Context.Image.Metadata.IptcProfile = null;
    }

    [Filter]
    public void RemoveXmp()
    {
        Context.Image.Metadata.XmpProfile = null;
    }

    [Filter]
    public void RemoveMetadata()
    {
        RemoveExif();
        RemoveIcc();
        RemoveIptc();
        RemoveXmp();
    }
}
