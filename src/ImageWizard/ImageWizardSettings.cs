using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard
{
    public class ImageWizardSettings
    {
        public ImageWizardSettings()
        {
            AllowUnsafeUrl = false;
        }

        public bool AllowUnsafeUrl { get; set; }


        public string Key { get; set; }



    }
}
