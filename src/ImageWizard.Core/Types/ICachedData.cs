// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

/// <summary>
/// ICachedData
/// </summary>
public interface ICachedData
{
    /// <summary>
    /// Metadata
    /// </summary>
    /// <returns></returns>
    IMetadata Metadata { get; }

    /// <summary>
    /// OpenReadAsync
    /// </summary>
    /// <returns></returns>
    Task<Stream> OpenReadAsync();
}
