// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// ImageWizardOptionsExtensions
/// </summary>
public static class ImageWizardOptionsExtensions
{
    /// <summary>
    /// WhenLoaderFailedUseExistingCachedData
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ImageWizardOptions WhenLoaderFailedUseExistingCachedData(this ImageWizardOptions options)
    {
        options.FallbackHandler = (state, url, cachedData) =>
                                    {
                                        //use the existing cached data if available?
                                        if (cachedData != null)
                                        {
                                            return cachedData;
                                        }

                                        return null;
                                    };

        return options;
    }
}
