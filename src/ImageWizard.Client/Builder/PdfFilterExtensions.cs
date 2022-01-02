using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// PdfFilterExtensions
    /// </summary>
    public static class PdfFilterExtensions
    {
        public static Image PageToImage(this Pdf pdf, int pageIndex)
        {
            return new Image(pdf.Filter($"pagetoimage({pageIndex})"));
        }

        public static Image PageToImage(this Pdf pdf, int pageIndex, int width, int height)
        {
            return new Image(pdf.Filter($"pagetoimage({pageIndex},{width},{height})"));
        }
    }
}
