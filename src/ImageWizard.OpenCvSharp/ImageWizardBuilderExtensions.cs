using ImageWizard.ImageSharp.Filters;

namespace ImageWizard.OpenCvSharp
{
    public static class ImageWizardBuilderExtensions
    {
        public static IImageWizardBuilder AddOpenCvSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<OpenCvSharpPipeline>(mimeTypes);

            return builder;
        }
    }
}