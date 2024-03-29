﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Mvc;
using Piranha.Extend.Fields;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using ImageWizard.Client;

namespace ImageWizard.Piranha;

public static class Extensions
{
    public static IFilter Fetch(this ILoader imageLoader, ImageField imageField)
    {
        //UrlDecode: fix whitespace handling for correct signature check
        IUrlHelper urlHelper = imageLoader.ServiceProvider.GetRequiredService<IUrlHelper>();

        return imageLoader.FetchLocalFile(urlHelper.Content(HttpUtility.UrlDecode(imageField.Media.PublicUrl)));
    }
}
