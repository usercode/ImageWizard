// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Loaders;

public class FileLoaderOptions : LoaderOptions
{
    public FileLoaderOptions()
    {
        RefreshMode = LoaderRefreshMode.EveryTime;
        Folder = "FileStorage";
    }

    public string Folder { get; set; }
}
