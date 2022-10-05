// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// VideoFilterExtensions
/// </summary>
public static class VideoFilterExtensions
{
    public static Video GetFrame(this Video video)
    {
        video.Filter($"frame()");

        return video;
    }
   
}
