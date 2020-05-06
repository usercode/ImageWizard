using ImageWizard.Core.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Base
{
    public abstract class FilterContext
    {
        /// <summary>
        /// NoCache
        /// </summary>
        public bool NoImageCache { get; set; }

        /// <summary>
        /// ClientHints
        /// </summary>
        public ClientHints ClientHints { get; set; }
    }
}
