using ImageWizard.Client.Builder;
using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageEffectsExtensions
    /// </summary>
    public static class SvgEffectsExtensions
    {
        public static ISvgFilter Blur(this ISvgFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"blur()");

            return imageUrlBuilder;
        }

        public static ISvgFilter Blur(this ISvgFilter imageUrlBuilder, int radius)
        {
            imageUrlBuilder.Filter($"blur({radius})");

            return imageUrlBuilder;
        }

        public static ISvgFilter Grayscale(this ISvgFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"grayscale()");

            return imageUrlBuilder;
        }

        public static ISvgFilter Grayscale(this ISvgFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"grayscale({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static ISvgFilter Rotate(this ISvgFilter imageUrlBuilder, double angle)
        {
            imageUrlBuilder.Filter($"rotate({angle.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static ISvgFilter Invert(this ISvgFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter($"invert()");

            return imageUrlBuilder;
        }

        public static ISvgFilter Saturate(this ISvgFilter imageUrlBuilder, double value)
        {
            imageUrlBuilder.Filter($"saturate({value.ToUrlString()})");

            return imageUrlBuilder;
        }

        public static ISvgFilter RemoveSize(this ISvgFilter imageUrlBuilder)
        {
            imageUrlBuilder.Filter("removesize()");

            return imageUrlBuilder;
        }
    }
}
