// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Utils;

public static class Base64Url
{
    public static string Encode(byte[] buffer)
    {
        return Convert.ToBase64String(buffer)
                            .TrimEnd('=')
                            .Replace('+', '-')
                            .Replace('/', '_');
    }

    public static byte[] Decode(string value)
    {
        string incoming = value
                                .Replace('_', '/')
                                .Replace('-', '+');

        switch (value.Length % 4)
        {
            case 2: incoming += "=="; break;
            case 3: incoming += "="; break;
        }

        byte[] bytes = Convert.FromBase64String(incoming);

        return bytes;
    }
}
