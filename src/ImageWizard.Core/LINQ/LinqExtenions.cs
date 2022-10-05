// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public static class LinqExtenions
{
    public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach(T item in items)
        {
            action(item);
        }
    }
}
