// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.ImageSharp;

public interface IImageFormat
{
    string MimeType { get; }

    Task SaveImageAsync(Image image, Stream stream);
}
