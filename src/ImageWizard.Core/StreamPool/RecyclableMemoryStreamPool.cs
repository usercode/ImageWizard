// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
