// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace ImageWizard.Client.TagHelpers;

/// <summary>
/// YoutubeTagHelper
/// </summary>
public class YoutubeTagHelper : TagHelper
{
    public YoutubeTagHelper(IImageWizardUrlBuilder imageUrlBuilder)
    {
        UrlBuilder = imageUrlBuilder;

        Width = 1920;
        Height = 1080;

        UseNoCookie = true;
    }

    /// <summary>
    /// Width
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Height
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Grayscale
    /// </summary>
    public bool Grayscale { get; set; }

    /// <summary>
    /// Blur
    /// </summary>
    public bool Blur { get; set; }

    /// <summary>
    /// VideoId
    /// </summary>
    public string VideoId { get; set; }

    /// <summary>
    /// Use the youtube-nocookie domain. (default: true)
    /// </summary>
    public bool UseNoCookie { get; set; }

    /// <summary>
    /// IImageWizardUrlBuilder
    /// </summary>
    private IImageWizardUrlBuilder UrlBuilder { get; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        string css = ReadResource("ImageWizard.Client.wwwroot.youtube.youtube.css");
        string js = ReadResource("ImageWizard.Client.wwwroot.youtube.youtube.js");
        string svg = ReadResource("ImageWizard.Client.wwwroot.youtube.youtube.svg");

        string youtubeBaseUrl = UseNoCookie ? "https://www.youtube-nocookie.com" : "https://www.youtube.com";

        XElement youtubeTag = new XElement("iframe",
                    new XAttribute("src", $"{youtubeBaseUrl}/embed/{VideoId}"),
                    new XAttribute("frameborder", "0"),
                    new XAttribute("allow", "autoplay; encrypted-media"),
                    new XAttribute("allowfullscreen", "allowfullscreen"),
                    new XText("")
                    );

        XElement responsiveTag = new XElement("div", 
                                        new XAttribute("class", "imagewizard-responsive-video"),
                                        youtubeTag);

        output.PreElement.AppendHtml($"<style>{css}</style>");
        output.PreElement.AppendHtml($"<script>{js}</script>");
        
        string elementId = $"yt_{context.UniqueId}_{VideoId}";

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("id", elementId);
        output.Attributes.SetAttribute("class", "imagewizard-youtube");
        output.Attributes.SetAttribute("data-embeded", HttpUtility.HtmlEncode(responsiveTag.ToString()));

        Image image = UrlBuilder
                            .Youtube(VideoId)
                            .Resize(Width, Height);

        if (Grayscale)
        {
            image.Grayscale();
        }

        if (Blur)
        {
            image.Blur();
        }

        output.Content.AppendHtml($"<img src=\"{image.BuildUrl()}\" class=\"imagewizard-youtube-image\" onclick=\"openYoutube('{elementId}')\" />");
        output.Content.AppendHtml($"<div>{svg}</div>");
    }

    private string ReadResource(string name)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        using StreamReader readerCss = new StreamReader(stream);

        return readerCss.ReadToEnd();
    }
}
