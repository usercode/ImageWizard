// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;

namespace ImageWizard.Client;

public interface IUrlBuilder
{
    public ImageWizardClientSettings Settings { get; }

    public IServiceProvider ServiceProvider { get; }
}
