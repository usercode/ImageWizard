// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public static class StreamExtensions
{
    public static async Task<bool> TryCopyToAsync(this Stream stream, Stream destination, long maxLength)
    {
        int bufferLength = 4096 * 20; //81920
        byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferLength);

        try
        {
            int length = 0;
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, bufferLength))) != 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, bytesRead));

                length += bytesRead;

                if (length > maxLength)
                {
                    return false;
                }
            }

            return true;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
