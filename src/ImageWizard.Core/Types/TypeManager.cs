// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// TypeManager
/// </summary>
public sealed class TypeManager
{
    private IDictionary<string, Type> LoaderTypes = new Dictionary<string, Type>();

    public IEnumerable<string> GetAllKeys()
    {
        return LoaderTypes.Keys;
    }

    /// <summary>
    /// Determines whether the TypeManager contains an element with the specified key.
    /// </summary>
    public bool ContainsKey(string key)
    {
        return LoaderTypes.ContainsKey(key);
    }

    /// <summary>
    /// Get
    /// </summary>
    public Type Get(string key)
    {
        if (LoaderTypes.TryGetValue(key, out Type? loaderType) == false)
        {
            throw new Exception($"Type was not found: {key}");
        }

        return loaderType;
    }

    /// <summary>
    /// Register
    /// </summary>
    public void Register<T>(string key)
    {
        Register(key, typeof(T));
    }

    /// <summary>
    /// Register
    /// </summary>
    public void Register(string key, Type type)
    {
        LoaderTypes[key] = type;
    }
}
