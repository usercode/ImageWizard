// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Utils;

/// <summary>
/// IUrlSignature
/// </summary>
public interface IUrlSignature
{
    /// <summary>
    /// Encrypt
    /// </summary>
    /// <param name="key"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    string Encrypt(byte[] key, ImageWizardRequest request);
}
