﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.ImageFilters.Base.Attributes
{
    /// <summary>
    /// It marks a parameter which have to be multiply with the DPR (device pixel ratio) value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DPRAttribute : Attribute
    {
    }
}
