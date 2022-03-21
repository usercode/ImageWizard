// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches;

/// <summary>
/// ICacheKey
/// </summary>
public interface ICacheKey
{
    /// <summary>
    /// Create
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    string Create(string input);
}
