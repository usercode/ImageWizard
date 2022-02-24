// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// SvgEffectsExtensions
    /// </summary>
    public static class SvgEffectsExtensions
    {
        public static Svg Blur(this Svg svg)
        {
            svg.Filter($"blur()");

            return svg;
        }

        public static Svg Blur(this Svg svg, int radius)
        {
            svg.Filter($"blur({radius})");

            return svg;
        }

        public static Svg Grayscale(this Svg svg)
        {
            svg.Filter($"grayscale()");

            return svg;
        }

        public static Svg Grayscale(this Svg svg, double value)
        {
            svg.Filter($"grayscale({value.ToUrlString()})");

            return svg;
        }

        public static Svg Rotate(this Svg svg, double angle)
        {
            svg.Filter($"rotate({angle.ToUrlString()})");

            return svg;
        }

        public static Svg Invert(this Svg svg)
        {
            svg.Filter($"invert()");

            return svg;
        }

        public static Svg Saturate(this Svg svg, double value)
        {
            svg.Filter($"saturate({value.ToUrlString()})");

            return svg;
        }

        public static Svg RemoveSize(this Svg svg)
        {
            svg.Filter("removesize()");

            return svg;
        }
    }
}
