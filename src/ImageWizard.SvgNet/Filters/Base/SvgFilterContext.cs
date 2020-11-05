using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.ImageProcessing;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using ImageWizard.Settings;
using ImageWizard.SvgNet;
using Svg;
using Svg.FilterEffects;
using System;
using System.Collections.Generic;
using System.IO;
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
        public SvgFilterContext(ProcessingPipelineContext processingContext, SvgDocument image)
            : base(processingContext)
        {
            Image = image;
            NoImageCache = false;
            Filters = new List<SvgFilterPrimitive>();
        }

        /// <summary>
        /// Image
        /// </summary>
        public SvgDocument Image { get; }

        /// <summary>
        /// Filters
        /// </summary>
        public IList<SvgFilterPrimitive> Filters { get; set; }

        public override async Task<ImageResult> BuildResultAsync()
        {
            //apply filters
            if (Filters.Any())
            {
                var defs = Image.Children.GetSvgElementOf<SvgDefinitionList>();

                if (defs == null)
                {
                    defs = new SvgDefinitionList();
                    Image.Children.Add(defs);
                }

                var filterElement = new Svg.FilterEffects.SvgFilter();
                filterElement.ID = "filter01";

                defs.Children.Add(filterElement);

                foreach (var f in Filters)
                {
                    filterElement.Children.Add(f);
                }

                Image.CustomAttributes.Add("filter", $"url(#{filterElement.ID})");
            }

            MemoryStream mem = new MemoryStream();
            Image.Write(mem);

            return new ImageResult(mem.ToArray(), MimeTypes.Svg);
        }
    }
}
