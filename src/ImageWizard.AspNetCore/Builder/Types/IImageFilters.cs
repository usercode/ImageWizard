using ImageWizard.SharedContract.FilterTypes;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore.Builder.Types
{
    public interface IImageFilters : IImageBuildUrl
    {
        IImageFilters Crop(int width, int heigth);

        IImageFilters Crop(int x, int y, int width, int heigth);

        IImageFilters Crop(double width, double heigth);

        IImageFilters Crop(double x, double y, double width, double heigth);

        IImageFilters Resize(int size);

        IImageFilters Resize(int width, int height);

        IImageFilters Resize(int width, int height, ResizeMode mode);

        IImageFilters Trim();

        IImageFilters Grayscale();

        IImageFilters BlackWhite();

        IImageFilters Rotate(RotateMode mode);

        IImageFilters Flip(FlipMode flippingMode);

        IImageFilters DPR(double value);

        IImageBuildUrl Jpg();

        IImageBuildUrl Jpg(int quality);

        IImageBuildUrl Png();

        IImageBuildUrl Gif();

        IImageBuildUrl Bmp();
    }
}
