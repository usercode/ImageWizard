using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageWizard.Filters
{
    public abstract class SvgFilter : FilterBase<SvgFilterContext>
    {
        public SvgFilter()
        {

        }

        public override string Namespace => "svgnet";

    }
}
