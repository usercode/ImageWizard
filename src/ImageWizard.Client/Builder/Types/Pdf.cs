using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Client.Builder.Types
{
    public class Pdf : File
    {
        public Pdf(IFilter filter)
            : base(filter)
        {

        }
    }
}
