// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// ILastAccessCache
/// </summary>
public interface ILastAccessCache : ICache
{
    /// <summary>
    /// SetLastAccessAsync
    /// </summary>
    /// <returns></returns>
    Task SetLastAccessAsync(string key, DateTime dateTime);
}
