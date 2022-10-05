// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

public interface ILoader : IUrlBuilder
{
    IFilter LoadData(string loaderType, string loaderSource);
}
