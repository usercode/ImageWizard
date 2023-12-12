// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using ImageWizard.SvgNet;
using System.Xml.Linq;

namespace ImageWizard.Filters;

/// <summary>
/// FilterContext
/// </summary>
public class SvgFilterContext : FilterContext
{
    public SvgFilterContext(
        PipelineContext processingContext,
        XDocument image)
        : base(processingContext)
    {
        Document = image;

        Root = Document.Root ?? throw new ArgumentNullException();
    }

    /// <summary>
    /// Image
    /// </summary>
    public XDocument Document { get; }

    /// <summary>
    /// Image
    /// </summary>
    public XElement Root { get; }

    /// <summary>
    /// Filters
    /// </summary>
    public IList<XElement> Filters { get; set; } = new List<XElement>();

    public override async Task<DataResult> BuildResultAsync()
    {
        //apply filters
        if (Filters.Any())
        {
            if (Root.Attribute("filter") == null)
            {
                Root.Add(new XAttribute("filter", $"url(#filter01)"));
            }

            XName defName = SvgConstants.SvgNs + "defs";
            XName filterName = SvgConstants.SvgNs + "filter";

            XElement? def = Root.Element(defName);

            if (def == null)
            {
                def = new XElement(defName);
                Root.Add(def);
            }

            XElement? filter = def.Element(filterName);

            if (filter == null)
            {
                filter = new XElement(filterName, new XAttribute("id", "filter01"));
                def.Add(filter);
            }

            foreach (XElement f in Filters)
            {
                filter.Add(f);
            }
        }

        Stream mem = ProcessingContext.StreamPool.GetStream();

        Document.Save(mem);

        mem.Seek(0, SeekOrigin.Begin);

        return new DataResult(mem, MimeTypes.Svg);
    }
}
