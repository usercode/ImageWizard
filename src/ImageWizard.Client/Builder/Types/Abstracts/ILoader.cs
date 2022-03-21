// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client;

public interface ILoader : IUrlBuilder
{
    IFilter LoadData(string loaderType, string loaderSource);
}
