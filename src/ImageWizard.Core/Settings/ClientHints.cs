using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageWizard.Core.Settings
{
    /// <summary>
    /// ClientHints
    /// </summary>
    public class ClientHints
    {
        public const string DPRHeader = "DPR";
        public const string WidthHeader = "Width";
        public const string ViewportWidthHeader = "Viewport-Width";

        public ClientHints(double[] allowedDPR)
        {
            AllowedDPR = allowedDPR;
        }

        private double[] AllowedDPR { get; }

        private double? _dpr;

        /// <summary>
        /// Current device pixel ratio value
        /// </summary>
        public double? DPR
        {
            get => _dpr;
            set
            {
                double? found = AllowedDPR
                                            .Cast<double?>()
                                            .FirstOrDefault(x => x >= value);

                if (found == null)
                {
                    found = AllowedDPR.LastOrDefault();
                }

                _dpr = found;
            }
        }

        /// <summary>
        /// Width
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// ViewportWidth
        /// </summary>
        public int? ViewportWidth { get; set; }
    }
}
