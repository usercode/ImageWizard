// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Piranha;
using Piranha.Extend;
using Piranha.Extend.Blocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Piranha;

public class PiranhaModule : IModule
{
    public string Author => "usercode";

    public string Name => "usercode/ImageWizard";

    public string Version => "1.0.0";

    public string Description => "";

    public string PackageUrl => "";

    public string IconUrl => "";

    public void Init()
    {
        App.Blocks.Register<ImageBlock>();
    }
}
