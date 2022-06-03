// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public enum FailedLoaderFallbackMode
{
    None = 0,
    UseExistingCachedData = 1,
    UseFallbackImage = 2
}
