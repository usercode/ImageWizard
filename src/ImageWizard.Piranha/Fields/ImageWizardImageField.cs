// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Piranha.Extend;
using Piranha.Extend.Fields;

namespace ImageWizard.Piranha.Fields;

[BlockType(Name = "Image", Category = "ImageWizard", Icon = "fas fa-quote-right", IsGeneric = true)]
public class ImageWizardImageField : Block
{
    public ImageField Image { get; set; }

    public ImageWizardImageField()
    {
        
    }
}
