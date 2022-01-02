using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters.Base
{
    public abstract class DocNETFilter : FilterBase<DocNETFilterContext>
    {
        public DocNETFilter()
        {

        }

        public override string Namespace => "docnet";

    }
}
