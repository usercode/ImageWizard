using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Client
{
    public class Video : File
    {
        public Video(IFilter filter)
            : base(filter)
        {

        }
    }
}
