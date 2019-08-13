using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Types
{
    public class CacheSettings
    {
        public bool NoCache { get; set; }
        public bool NoStore { get; set; }
        public string ETag { get; set; }
        public TimeSpan? MaxAge { get; set; }

    }
}
