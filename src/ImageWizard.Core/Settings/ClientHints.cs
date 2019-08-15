using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Settings
{
    public class ClientHints
    {
        public ClientHints()
        {

        }

        public double? DPR { get; set; }

        public int? Width { get; set; }

        public int? ViewportWidth { get; set; }
    }
}
