// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client;

/// <summary>
/// GravatarExtensions
/// </summary>
public static class GravatarExtensions
{
    public static Image Gravatar(this ILoader imageUrlBuilder, string email)
    {
        string GetHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        byte[] hashBuffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLower()));

        return new Image(imageUrlBuilder.LoadData("gravatar", GetHashString(hashBuffer)));
    }
}
