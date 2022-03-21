// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Client;

public class Image : File
{
    public Image(IFilter filter)
        : base(filter)
    {

    }
}
