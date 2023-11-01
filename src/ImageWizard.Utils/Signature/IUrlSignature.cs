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
    string Encrypt(byte[] key, ImageWizardRequest request);
}
