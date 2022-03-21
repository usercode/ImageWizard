using Xunit;

namespace ImageWizard.Tests;

public class ImageWizardUrlTest
{
    [Fact]
    public void ParseNoFilters()
    {
        bool result = ImageWizardUrl.TryParse("fetch/https://domain.tld/image.png", out ImageWizardUrl url);

        Assert.True(result);
        Assert.Equal("fetch/https://domain.tld/image.png", url.Path);
        Assert.Equal("fetch", url.LoaderType);
        Assert.Equal("https://domain.tld/image.png", url.LoaderSource);
        Assert.Empty(url.Filters);
    }

    [Fact]
    public void ParseWithOneFilters()
    {
        bool result = ImageWizardUrl.TryParse("resize(100,100)/fetch/https://domain.tld/image.png", out ImageWizardUrl url);

        Assert.True(result);
        Assert.Equal("resize(100,100)/fetch/https://domain.tld/image.png", url.Path);
        Assert.Equal("fetch", url.LoaderType);
        Assert.Equal("https://domain.tld/image.png", url.LoaderSource);
        Assert.Single(url.Filters);
        Assert.Equal("resize(100,100)", url.Filters[0]);
    }

    [Fact]
    public void ParseWithTwoFilters()
    {
        bool result = ImageWizardUrl.TryParse("resize(100,100)/blur()/fetch/https://domain.tld/image.png", out ImageWizardUrl url);

        Assert.True(result);
        Assert.Equal("resize(100,100)/blur()/fetch/https://domain.tld/image.png", url.Path);
        Assert.Equal("fetch", url.LoaderType);
        Assert.Equal("https://domain.tld/image.png", url.LoaderSource);
        Assert.Equal(2, url.Filters.Length);
        Assert.Equal("resize(100,100)", url.Filters[0]);
        Assert.Equal("blur()", url.Filters[1]);
    }
}
