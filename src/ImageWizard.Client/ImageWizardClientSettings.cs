using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public class ImageWizardClientSettings : ImageWizardBaseOptions
    {
        public ImageWizardClientSettings()
        {
            UseUnsafeUrl = false;
            Enabled = true;
            BaseUrl = ImageWizardDefaults.BasePath;
        }

        public bool Enabled { get; set; }

        public bool UseUnsafeUrl { get; set; }

        public string BaseUrl { get; set; }

        public string Host { get; set; }
        
    }
}
