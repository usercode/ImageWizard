// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Utils;

public abstract class ImageWizardBaseOptions
{
    public ImageWizardBaseOptions()
    {
        Key = Array.Empty<byte>();
    }

    /// <summary>
    /// Key
    /// </summary>
    public byte[] Key { get; set; }
}
