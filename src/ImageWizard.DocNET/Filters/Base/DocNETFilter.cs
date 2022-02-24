// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters.Base
{
    public abstract class DocNETFilter : Filter<DocNETFilterContext>
    {
        public DocNETFilter()
        {

        }

        public override string Namespace => "docnet";

    }
}
