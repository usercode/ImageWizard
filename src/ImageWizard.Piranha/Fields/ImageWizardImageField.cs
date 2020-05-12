using Microsoft.CodeAnalysis.CSharp.Syntax;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Piranha.Fields
{
    [BlockType(Name = "Image", Category = "ImageWizard", Icon = "fas fa-quote-right", IsGeneric = true)]
    public class ImageWizardImageField : Block
    {
        public ImageField Image { get; set; }

        public ImageWizardImageField()
        {
            
        }
    }
}
