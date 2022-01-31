namespace ImageWizard
{
    public class ImageWizardAppOptions
    {
        public ImageWizardAppOptions()
        {
            AddMetadata = false;
            MetadataCopyright = string.Empty;
        }

        public bool UseAnalytics { get; set; }
        public bool AddMetadata { get; set; }
        public string MetadataCopyright { get; set; }
    }
}
