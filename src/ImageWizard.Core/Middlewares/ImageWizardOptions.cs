// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using ImageWizard.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ImageWizard;

public delegate ICachedData? FallbackHandler(ImageWizardUrl url, ICachedData? existingCachedData);

/// <summary>
/// ImageWizardOptions
/// </summary>
public class ImageWizardOptions : ImageWizardBaseOptions
{
    public ImageWizardOptions()
    {
        UseETag = true;
        UseClientHints = false;
        AllowUnsafeUrl = false;
        UseAcceptHeader = false;
        RefreshLastAccessInterval = TimeSpan.FromDays(1);

        Key = string.Empty;

        AllowedDPR = ImageWizardDefaults.AllowedDPR;

        CacheControl = new CacheControl();
    }

    /// <summary>
    /// CacheControl
    /// </summary>
    public CacheControl CacheControl { get; }

    /// <summary>
    /// Allows unsafe url.
    /// </summary>
    public bool AllowUnsafeUrl { get; set; }

    /// <summary>
    /// Selects automatically the compatible mime type by request header. (Default: false)
    /// </summary>
    public bool UseAcceptHeader { get; set; }

    /// <summary>
    /// Uses ETag. (Default: true)
    /// </summary>
    public bool UseETag { get; set; }

    /// <summary>
    /// Duration when the last-access time (metadata) should be refreshed.
    /// </summary>
    public TimeSpan? RefreshLastAccessInterval { get; set; }

    /// <summary>
    /// Use client hints. (Default: false)
    /// </summary>
    public bool UseClientHints { get; set; }        

    /// <summary>
    /// Allowed DPR values. (Default: 1.0, 1.5, 2.0, 3.0)
    /// </summary>
    public double[] AllowedDPR { get; set; }

    /// <summary>
    /// FallbackHandler
    /// </summary>
    public FallbackHandler? FallbackHandler { get; set; }

    /// <summary>
    /// Generates random 64 byte key.
    /// </summary>
    public void GenerateRandomKey()
    {
        //generate random key
        byte[] keyBuffer = new byte[64];
        RandomNumberGenerator.Create().GetBytes(keyBuffer);

        Key = WebEncoders.Base64UrlEncode(keyBuffer);
    }
}
