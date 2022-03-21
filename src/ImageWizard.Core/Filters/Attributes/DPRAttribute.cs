// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Attributes;

/// <summary>
/// It marks a parameter which have to be multiply with the DPR (device pixel ratio) value.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class DPRAttribute : Attribute
{
}
