using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Settings;
using ImageWizard.Settings;
using ImageWizard.SvgNet;
using Svg;
using Svg.FilterEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageWizard.Filters
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public class SvgFilterContext : FilterContext
    {
        public SvgFilterContext(ImageWizardOptions settings, SvgDocument image)
        {
            Settings = settings;
            Image = image;
            NoImageCache = false;
            Filters = new List<SvgFilterPrimitive>();
        }

        /// <summary>
        /// Settings
        /// </summary>
        public ImageWizardOptions Settings { get; }

        /// <summary>
        /// Image
        /// </summary>
        public SvgDocument Image { get; }

        /// <summary>
        /// Filters
        /// </summary>
        public IList<SvgFilterPrimitive> Filters { get; set; }
    }
}
