// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
