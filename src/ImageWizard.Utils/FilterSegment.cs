// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Utils;

/// <summary>
/// FilterSegment
/// </summary>
public readonly struct FilterSegment
{
    public static FilterSegment FromFilter(string filter)
    {
        int pos = filter.IndexOf('(');

        return new FilterSegment(filter, filter[..pos], filter[pos..]);
    }

    public FilterSegment(string fullname, string name, string parameter)
    {
        Fullname = fullname;
        Name = name;
        Parameter = parameter;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Parameter
    /// </summary>
    public string Parameter { get; }

    /// <summary>
    /// Fullname
    /// </summary>
    public string Fullname { get; }

    public override string ToString()
    {
        return Fullname;
    }

    public static implicit operator FilterSegment(string filter) => FromFilter(filter);
}
