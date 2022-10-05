// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
