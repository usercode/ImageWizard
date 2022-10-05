// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Loaders;

/// <summary>
/// HttpHeaderItem
/// </summary>
public class HttpHeaderItem
{
    public HttpHeaderItem(string name, string value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }
}
