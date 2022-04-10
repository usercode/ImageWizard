// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

/// <summary>
/// InterceptorInvoker
/// </summary>
public class InterceptorInvoker : IImageWizardInterceptor
{
    public InterceptorInvoker(IEnumerable<IImageWizardInterceptor> items)
    {
        Items = items;
    }

    /// <summary>
    /// Items
    /// </summary>
    private IEnumerable<IImageWizardInterceptor> Items { get; }

    public void OnCachedDataCreated(ICachedData cachedData)
    {
        Items.Foreach(x => x.OnCachedDataCreated(cachedData));
    }

    public void OnCachedDataDeleted(ICachedData cachedData)
    {
        Items.Foreach(x => x.OnCachedDataDeleted(cachedData));
    }

    public void OnCachedDataSent(HttpResponse response, ICachedData cachedData, bool notModified)
    {
        Items.Foreach(x => x.OnCachedDataSent(response, cachedData, notModified));
    }

    public void OnInvalidSignature()
    {
        Items.Foreach(x => x.OnInvalidSignature());
    }

    public void OnUnsafeSignature()
    {
        Items.Foreach(x => x.OnUnsafeSignature());
    }

    public void OnValidSignature()
    {
        Items.Foreach(x => x.OnValidSignature());
    }
}
