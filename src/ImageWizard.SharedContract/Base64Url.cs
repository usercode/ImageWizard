using System;

namespace ImageWizard.SharedContract
{
    /// <summary>
    /// Base64Url
    /// </summary>
    public class Base64Url
    {
        /// <summary>
        /// FromBase64Url
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] FromBase64Url(string value)
        {
            return Convert.FromBase64String(
                                            value
                                                .Replace('-', '+')
                                                .Replace('_', '/')
                );
        }

        /// <summary>
        /// ToBase64Url
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToBase64Url(byte[] data)
        {
            return Convert.ToBase64String(data)
                                .Replace('+', '-')
                                .Replace('/', '_')
                                .Substring(0, 27); //remove padding '='
        }
    }
}
