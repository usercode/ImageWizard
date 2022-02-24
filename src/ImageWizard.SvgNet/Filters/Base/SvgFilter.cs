// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageWizard.Filters
{
    public abstract class SvgFilter : Filter<SvgFilterContext>
    {
        public SvgFilter()
        {

        }

        public override string Namespace => "svgnet";

    }
}
