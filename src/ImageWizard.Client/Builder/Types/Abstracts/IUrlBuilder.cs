// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

public interface IUrlBuilder
{
    public ImageWizardClientSettings Settings { get; }

    public IServiceProvider ServiceProvider { get; }
}
