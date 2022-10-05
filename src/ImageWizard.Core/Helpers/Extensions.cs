// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Net.Http.Headers;

namespace ImageWizard;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        MemoryStream mem = new MemoryStream();

        await stream.CopyToAsync(mem);

        return mem.ToArray();
    }

    public static byte[] ToByteArray(this Stream stream)
    {
        MemoryStream mem = new MemoryStream();

        stream.CopyTo(mem);

        return mem.ToArray();
    }

    public static MemoryStream ToMemoryStream(this Stream stream)
    {
        MemoryStream mem = new MemoryStream();

        stream.CopyTo(mem);

        mem.Seek(0, SeekOrigin.Begin);

        return mem;
    }

    public static string GetTagUnquoted(this EntityTagHeaderValue value)
    {
        return value.Tag.GetTagUnquoted();
    }

    public static string GetTagUnquoted(this string value)
    {
        return value[1..^1];
    }
}
