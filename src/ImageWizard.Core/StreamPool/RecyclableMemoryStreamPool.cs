// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.IO;

namespace ImageWizard;

public class RecyclableMemoryStreamPool : IStreamPool
{
    private static readonly RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();

    public RecyclableMemoryStreamPool()
    {

    }

    public Stream GetStream()
    {            
        return manager.GetStream();
    }
}
