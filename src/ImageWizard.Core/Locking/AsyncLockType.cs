// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Locking;

/// <summary>
/// AsyncLockType
/// </summary>
public enum AsyncLockType
{
    /// <summary>
    /// Read
    /// </summary>
    Read,

    /// <summary>
    /// Write
    /// </summary>
    Write
}
