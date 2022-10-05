// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public class MemoryStreamPool : IStreamPool
{
    public Stream GetStream()
    {
        return new MemoryStream();
    }
}
