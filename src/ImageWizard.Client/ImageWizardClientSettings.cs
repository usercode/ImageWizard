using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public class ImageWizardClientSettings
    {
        public ImageWizardClientSettings()
        {
            UseUnsafeUrl = false;
            Enabled = true;
            BaseUrl = ImageWizardDefaults.BasePath;
            Key = null;
        }

        public bool UseUnsafeUrl { get; set; }

        public string BaseUrl { get; set; }

        public string Key { get; set; }

        public bool Enabled { get; set; }
    }
}
