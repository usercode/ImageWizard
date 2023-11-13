// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// ImageBuilderYouTubeExtensions
/// </summary>
public static class YouTubeExtensions
{
    public static Image YouTube(this ILoader imageUrlBuilder, string id)
    {
        return new Image(imageUrlBuilder.LoadData("youtube", id));
    }
}
