// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

namespace SourceGenerator;

public class TypeElement
{
    public static readonly TypeElement String = Get("string");

    public static readonly TypeElement Guid = Get("Guid");

    public static readonly TypeElement Int16 = Get("short");

    public static readonly TypeElement Int32 = Get("int");

    public static readonly TypeElement Int64 = Get("long");

    public static readonly TypeElement Void = Get("void");

    public static TypeElement Get(string name) => new TypeElement(name);

    private TypeElement(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// IsNullable
    /// </summary>
    public bool IsNullable { get; private set; }

    /// <summary>
    /// Makes the type nullable.
    /// </summary>
    /// <returns></returns>
    public TypeElement Nullable()
    {
        IsNullable = true;

        return this;
    }

    public override string ToString()
    {
        if (IsNullable)
        {
            return $"{Name}?";
        }
        else
        {
            return Name;
        }
    }
}
