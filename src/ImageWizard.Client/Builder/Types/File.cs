// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

public abstract class File : IFilter
{
    public File(IFilter filter)
    {
        CurrentFilter = filter;
    }

    public IFilter CurrentFilter { get; set; }

    public ImageWizardClientSettings Settings => CurrentFilter.Settings;

    public IServiceProvider ServiceProvider => CurrentFilter.ServiceProvider;

    public string BuildUrl() => CurrentFilter.BuildUrl();

    public IFilter Filter(string filter) => CurrentFilter.Filter(filter);
}
