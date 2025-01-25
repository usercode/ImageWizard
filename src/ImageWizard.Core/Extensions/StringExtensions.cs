// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard;

public static class StringExtensions
{
    public static string CreateSHA256Hash(this string input)
    {
        int inputLength = Encoding.UTF8.GetByteCount(input);

        byte[] inputBuffer = ArrayPool<byte>.Shared.Rent(inputLength);

        try
        {
            Span<byte> inputBufferSpan = inputBuffer.AsSpan(0, inputLength);

            Encoding.UTF8.GetBytes(input, inputBufferSpan);

            Span<byte> hashBuffer = stackalloc byte[32];

            SHA256.HashData(inputBufferSpan, hashBuffer);

            string key = Convert.ToHexString(hashBuffer);

            return key;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(inputBuffer);
        }
    }
}
