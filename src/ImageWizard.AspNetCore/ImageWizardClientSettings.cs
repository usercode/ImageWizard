using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.AspNetCore
{
    public class ImageWizardClientSettings
    {
        public ImageWizardClientSettings()
        {
            Enabled = true;
            BaseUrl = "/image";
        }

        public string BaseUrl { get; set; }

        public string Key { get; set; }

        public bool Enabled { get; set; }
    }
}
