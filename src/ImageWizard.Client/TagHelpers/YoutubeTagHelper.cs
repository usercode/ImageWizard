using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace ImageWizard.Client.TagHelpers
{
    /// <summary>
    /// YoutubeTagHelper
    /// </summary>
    public class YoutubeTagHelper : TagHelper
    {
        public YoutubeTagHelper(IUrlHelper imageUrlBuilder)
        {
            UrlBuilder = imageUrlBuilder;

            Width = 1920;
            Height = 1080;
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
        /// VideoId
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// UrlBuilder
        /// </summary>
        private IUrlHelper UrlBuilder { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            string css = ReadResource("ImageWizard.Client.wwwroot.youtube.youtube.css");
            string js = ReadResource("ImageWizard.Client.wwwroot.youtube.youtube.js");
            string svg = ReadResource("ImageWizard.Client.wwwroot.youtube.youtube.svg");

            XElement youtubeTag = new XElement("iframe",
                        new XAttribute("src", $"https://www.youtube-nocookie.com/embed/{VideoId}"),
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

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("id", "y1");
            output.Attributes.SetAttribute("class", "imagewizard-youtube");
            output.Attributes.SetAttribute("data-embeded", HttpUtility.HtmlEncode(responsiveTag.ToString()));

            output.Content.AppendHtml($"<img src=\"{UrlBuilder.ImageWizard().Youtube(VideoId).Resize(Width, Height).BuildUrl()}\" class=\"imagewizard-youtube-image\" onclick=\"openYoutube('y1','{VideoId}')\" />");
            output.Content.AppendHtml($"<div>{svg}</div>");
        }

        private string ReadResource(string name)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            StreamReader readerCss = new StreamReader(stream);
            return readerCss.ReadToEnd();
        }
    }
}
