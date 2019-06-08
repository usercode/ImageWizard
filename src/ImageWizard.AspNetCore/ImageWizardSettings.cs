using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore
{
    public class ImageWizardSettings
    {
        public ImageWizardSettings()
        {
            Enabled = true;
        }

        public string BaseUrl { get; set; }

        public string Key { get; set; }

        public bool Enabled { get; set; }
    }
}
