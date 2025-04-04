﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Caches;

/// <summary>
/// ILastAccessCache
/// </summary>
public interface ILastAccessCache : ICache
{
    /// <summary>
    /// SetLastAccessAsync
    /// </summary>
    Task SetLastAccessAsync(string key, DateTime dateTime);
}
