// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;

namespace ImageWizard.Azure;

public class AzureBlobOptions : LoaderOptions
{
    public AzureBlobOptions()
    {
        RefreshMode = LoaderRefreshMode.EveryTime;
    }

    public string ConnectionString { get; set; }

    public string ContainerName { get; set; }
}
