// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public enum LoaderResultState
{
    Success = 0,
    NotModified = 1,
    NotFound = 2,
    Failed = 3
}
