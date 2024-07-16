// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

using Microsoft.CodeAnalysis;

namespace SourceGenerator;

/// <summary>
/// Modifier
/// </summary>
public class Modifier
{
    public static readonly Modifier Public = Get("public");
    public static readonly Modifier Private = Get("private");
    public static readonly Modifier Protected = Get("protected");
    public static readonly Modifier Internal = Get("internal");
    public static readonly Modifier File = Get("file");

    public static Modifier Get(string name) => new Modifier(name);

    public static Modifier From(Accessibility accessibility)
    {
        return accessibility switch
        {
            Accessibility.Public => Public,
            Accessibility.Internal => Internal,
            Accessibility.Protected => Protected,
            Accessibility.Private => Private,
            Accessibility.NotApplicable => Internal,
            _ => Internal
        };
    }

    private Modifier(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }
}
