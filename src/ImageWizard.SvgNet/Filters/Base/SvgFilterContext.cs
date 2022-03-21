// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using Svg;
using Svg.FilterEffects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters;

/// <summary>
/// FilterContext
/// </summary>
public class SvgFilterContext : FilterContext
{
    public SvgFilterContext(
        PipelineContext processingContext, 
        SvgDocument image)
        : base(processingContext)
    {
        Image = image;
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

    public override async Task<DataResult> BuildResultAsync()
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

        Stream mem = ProcessingContext.StreamPool.GetStream();

        Image.Write(mem);

        mem.Seek(0, SeekOrigin.Begin);

        return new DataResult(mem, MimeTypes.Svg);
    }
}
