// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Loaders;

/// <summary>
/// HttpHeaderItem
/// </summary>
public class HttpHeaderItem(string name, string value)
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; } = value;
}
