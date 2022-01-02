using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Client
{
    public abstract class File : IFileType, IFilter
    {
        public File(IFilter filter)
        {
            CurrentFilter = filter;
        }

        public IFilter CurrentFilter { get; set; }

        public ImageWizardClientSettings Settings => CurrentFilter.Settings;

        public IServiceProvider ServiceProvider => CurrentFilter.ServiceProvider;

        public string BuildUrl() => CurrentFilter.BuildUrl();

        public IFilter Filter(string filter) => CurrentFilter.Filter(filter);
    }
}
