# ImageWizard 3.2
A ASP.NET Core service / middleware to resize your images dynamically as alternative for thumbor.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/)
[![Docker](https://img.shields.io/docker/pulls/usercode/imagewizard)](https://hub.docker.com/r/usercode/imagewizard)

Demo: [imagewizard.net](https://imagewizard.net)

## Features
- any data loader:
	- Http (streaming mode)
	- File (use IFileProvider)
	- YouTube (get thumbnail)
	- Gravatar
	- OpenGraph
- any data caches:
	-  File
	-  Distributed cache
	-  MongoDB
- image filters: resize, crop, rotate,..
- common image effects like grayscale and blur are available 
- create your custom data filter
- pdf filters: page-to-image for documents
- url is protected by a HMACSHA256 signature to prevent DDoS attacks
- can handle the device pixel ratio (DPR)
- support for cache control and ETag
- enable range processing by http request
- use RecyclableMemoryStream for smarter memory management (IStreamPool)

## Example
<p align="center">
https://localhost/  <br/>
image/  <br/>
cGiAwFYGYWx0SzO0YyCidWIfkdlUYrVgBwbm7bcTOjE/  <br/>
resize(200,200)/grayscale()/jpg(90)/  <br/>
fetch/  <br/>
https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png
</p>

| Description         | Url segment |
|---------------------------------|-----------------|
| base path                       | "image" | 
| signature based on HMACSHA256   | "cGiAwFYGYWx0SzO0YyCidWIfkdlUYrVgBwbm7bcTOjE" or "unsafe" (if enabled) |
| any filters                     | "resize(200,200)/grayscale()/jpg(90)" |
| loader type                     | "fetch" |
| loader source                   | https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png | 

## Data loaders
| Name                | Loader type | Loader source | NuGet |
|-----------------------------------|-----------------|----|---|
| Http loader        | fetch       | absolute or relative url |  [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/) 
| File loader         | file        | relative path to file   | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/) 
| YouTube loader      | youtube     | video id                | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/)
| Gravatar loader     | gravatar    | encoded email address   | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/)
| OpenGraph loader     | opengraph  | absolute url   | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.OpenGraph.svg)](https://www.nuget.org/packages/ImageWizard.OpenGraph/)
| Azure loader       | azure       | relative path to file   | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Azure.svg)](https://www.nuget.org/packages/ImageWizard.Azure/)

## Data caches
| Name                 | Description        | NuGet     |
|----------------------|--------------------|-----------|
|  File cache         |                    |  [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/)  |
|  Distributed cache   | MS SQL, Redis      |  [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/)  |
|  MongoDB cache       | Use GridFS |  [![NuGet](https://img.shields.io/nuget/v/ImageWizard.MongoDB.svg)](https://www.nuget.org/packages/ImageWizard.MongoDB/) |

## Data pipelines

| Name  |  Mime type | NuGet |
|------------------------------------|-----------------|------------|
| ImageSharp | image/jpeg, image/png, image/gif, image/bmp, image/tga |  [![NuGet](https://img.shields.io/nuget/v/ImageWizard.ImageSharp.svg)](https://www.nuget.org/packages/ImageWizard.ImageSharp/)
| SkiaSharp  | image/jpeg, image/png, image/gif, image/bmp, image/webp  | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.SkiaSharp.svg)](https://www.nuget.org/packages/ImageWizard.SkiaSharp/)
| SvgNet     | image/svg+xml | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.SvgNet.svg)](https://www.nuget.org/packages/ImageWizard.SvgNet/)
| DocNET     | application/pdf | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.DocNET.svg)](https://www.nuget.org/packages/ImageWizard.DocNET/)

## Integrate into existing ASP.NET Core applications

```csharp
services.AddImageWizard();

//or

services.AddImageWizard(options => 
                       {
                           options.AllowUnsafeUrl = true;             
                           options.AllowedDPR = new[] { 1.0, 1.5, 2.0, 3.0, 4.0 };
                           options.Key = "DEMO-KEY..."; //64 byte key encoded in Base64Url
                           options.UseETag = true;                                                
                           options.CacheControl.IsEnabled = true;
                           options.CacheControl.MaxAge = TimeSpan.FromDays(365);
                           options.CacheControl.MustRevalidate = false;
                           options.CacheControl.Public = true;
                           options.CacheControl.NoCache = false;
                           options.CacheControl.NoStore = false;
                           //select automatically the compatible mime type by request header
                           options.UseAcceptHeader = true;
                       })
            //registers ImageSharp pipeline for specified mime types
           .AddImageSharp(c => c
                .WithMimeTypes(MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp)
                .WithOptions(x =>
                            {
                                x.ImageMaxHeight = 4000;
                                x.ImageMaxWidth = 4000;
                            })
                //Adds your custom filters
                .WithFilter<BlurFilter>()
                .WithPreProcessing(x =>
                            {
                                x.Image.Mutate(m => m.AutoOrient());
                            })
                .WithPostProcessing(x =>
                            {
			    	//blur all images
                                x.Image.Mutate(m => m.Blur());
                              
                                //override target format
                                x.ImageFormat = new JpegFormat();
				
				//override metadata
				x.Image.Metadata.ExifProfile = new ExifProfile();
                        	x.Image.Metadata.ExifProfile.SetValue(ExifTag.Copyright, "ImageWizard");
                            }))
           .AddSkiaSharp(MimeTypes.WebP)
           .AddSvgNet()
	   .AddDocNET()
           //uses file cache (relative or absolute path)
           .SetFileCache(options => options.Folder = "FileCache") 
           //or MongoDB cache
           .SetMongoDBCache(options => options.Hostname = "localhost")
           //or distributed cache
           .SetDistributedCache()
           //adds some loaders
           .AddFileLoader(options => options.Folder = "FileStorage")
           .AddHttpLoader(options => 
          {
                                   //checks every time for a new version of the original image.
                                   options.RefreshMode = DataLoaderRefreshMode.EveryTime;
                                   
                                   //set base url for relative urls
                                   options.DefaultBaseUrl = "https://mydomain";
                                   
                                   //allow only relative urls 
                                   //(use base url from request or DefaultBaseUrl from options)
                                   options.AllowAbsoluteUrls = false;
                                   
                                   //allow only specified hosts
                                   options.AllowedHosts = new [] { "mydomain" };
                                   
                                   //adds custom http header like apikey to prevent 
                                   //that user can download the original image
                                   options.SetHeader("ApiKey", "123456");
     })
           .AddYoutubeLoader()
           .AddGravatarLoader()
	   .AddOpenGraphLoader()
           .AddAnalytics()
           ;
```

```csharp
app.UseEndpoints(x => x.MapImageWizard());

or

//default path: /image
app.UseImageWizard(x =>
		{
			//default path: /analytics
			x.UseAnalytics();
		});
```

## Create custom filter

- add a public method which is marked with the filter attribute
  - at url level are the following types possible for method overloading: 
    - integer ("0")
    - floating-point number ("0.0")
    - bool ("True" or "False")
    - enum (value)
    - string ('Hello')

```csharp
 public class BackgroundColorFilter : ImageSharpFilter
    {
        //use dependency injection
        public BackgroundColorFilter(ILogger<BackgroundColorFilter> logger)
        {
          //...
        }
        
        [Filter]
        public void BackgroundColor(byte r, byte g, byte b)
        {
            Context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
        }

        [Filter]
        public void BackgroundColor(float r, float g, float b)
        {
            Context.Image.Mutate(m => m.BackgroundColor(new Rgba32(r, g, b)));
        }
    }
```

Register filter:

```csharp
services.AddImageWizard()
	.AddImageSharp(c => c.WithFilter<BackgroundColorFilter>());
```

URL segments: 

```csharp
"/backgroundcolor(255,255,255)/"
"/backgroundcolor(1.0,1.0,1.0)/"
```

### How to use the DPR (device pixel ratio) attribute

- use dpr filter to set the value or enable the client hints
- all parameter with DPR attribute will be multiplied by the DPR factor

```csharp
 public class ResizeFilter : ImageSharpFilter
 {
      [Filter]
      public void Resize([DPR]int width, [DPR]int height)
      {
          Context.Image.Mutate(m => m.Resize(width, height));
      }
 }
```

URL segment: 
```csharp
"/dpr(2.0)/resize(200,100)/"             //calls resize filter with the resolution 400 x 200
or 
"/resize(200,100)/" + client hints  
```
Response header:
```csharp
Content-DPR: 2
```

### How to use optional parameters

- use rather method overloading if possible
- if all parameter have default values you can set all parameters by name

Example:

```csharp
 public class TextFilter : ImageSharpFilter
 {
      [Filter]
      public void DrawText(int x = 0, int y = 0, string text = "", int size = 12, string font = "Arial")
      {
          Context.Image.Mutate(m =>
          {
              m.DrawText(
                  text,
                  new Font(SystemFonts.Find(font), size),
                  Rgba32.Black,
                  new PointF(x, y));
          });
      }
 }
```

URL segment: 
```csharp
"/drawtext(text='Hello',x=10,y=20)/"
```

## ASP.NET Core UrlBuilder

https://www.nuget.org/packages/ImageWizard.Client/

Example:

Add settings to the appsettings.json

```json
 "ImageWizard": {
    "BaseUrl": "https://<your-domain>/image",
    "Key": "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==",
    "Enabled": true
  }
```

Register settings to services

```csharp
services.Configure<ImageWizardClientSettings>(Configuration.GetSection("ImageWizard"));

services.AddImageWizardClient();

//or

services.AddImageWizardClient(options => 
{
    options.BaseUrl = "https://<your-domain>/image";
    options.Key = "..";
    options.Enabled = true;
    options.UseUnsafeUrl = false;
});
```
Create url with fluent api

```csharp
@Url
.ImageWizard()
//use HTTP loader
.Fetch("https://<your-domain>/test/picture.jpg")
//fetch local file from wwwroot folder (with fingerprint)
.FetchLocal("picture.jpg")
//or file loader
.File("test/picture.jpg")
//or azure
.Azure("image.jpg")
.AsImage()
.Resize(160,140,ResizeMode.Max)
.Blur()
.Grayscale()
.Jpg(90)
.BuildUrl()
```

Use dependency injection
```csharp
@IImageWizardUrlBuilder UrlBuilder

<img src="@UrlBuilder.FetchLocalFile("picture.jpg").AsImage().Resize(400, 200, ResizeMode.Max).Grayscale().BuildUrl()" />
```

Use IUrlHelper
```csharp
<img src="@Url.ImageWizard().FetchLocalFile("picture.jpg").AsImage().Resize(400, 200, ResizeMode.Max).Grayscale().BuildUrl()" />
```

## Processing pipelines
### ImageSharp
#### Image transformations
- resize(size)
- resize(width,height)
- resize(width,height,mode)
  - mode: min, max, crop, pad, stretch
- resize(width,height,mode,anchor)
  - anchor: center, top, bottom, left, right, topleft, topright, bottomleft, bottomright
- crop(width,height)
  - int for absolute values, 0.0 to 1.0 for relative values
- crop(x,y,width,height)
  - int for absolute values, 0.0 to 1.0 for relative values
- backgroundcolor(r,g,b)
  - int (0 to 255) or float (0.0 to 1.0)
- backgroundcolor(r,g,b,a)
  - int (0 to 255) or float (0.0 to 1.0)
- flip(type)
  - type: horizontal, vertical
- rotate(value) 
  - value: 90, 180 or 270
- grayscale()
- grayscale(amount)
  - amount: 0.0 to 1.0
- blackwhite()
- blur()
- invert()
- brightness(value)
- contrast(value)
- autoorient()
- drawtext(text='Hello',size=24,x=0.5,y=0.5)
  - string: raw ('Hello') or base64url (SGVsbG8)

#### Output formats
- jpg()
- jpg(quality)
- png()
- gif()
- tga()
- bmp()

#### Special options
- dpr(value) //set allowed device pixel ratio

### SkiaSharp
#### Image transformations
- resize(width,height)
- resize(width,height,mode)
  - mode: min, max, crop, pad, stretch
- crop(width,height)
  - 0.0 to 1.0 for relative values
- crop(x,y,width,height)
  - 0.0 to 1.0 for relative values
- flip(type)
  - type: horizontal, vertical
- rotate(value)
  - value: 0.0 to 360.0
- grayscale()
- blur()
- blur(radius)
- drawtext(text='Hello',size=24,x=0.5,y=0.5)
  - string: raw ('Hello') or base64url (SGVsbG8)
  
#### Output formats
- jpg()
- jpg(quality)
- png()
- gif()
- bmp()
- webp()

### DocNET
#### DocNET transformations
- pagetoimage(pageindex)
- pagetoimage(pageindex,width,height)
- subpages(pageFromIndex, pageToIndex)

### SvgNet
#### SVG transformations
- removesize()
- blur()
- blur(deviation)
  - value: float
- rotate(angle)
  - value: float


## Plugin for Piranha CMS 8.0

ImageWizard.Piranha [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Piranha.svg)](https://www.nuget.org/packages/ImageWizard.Piranha/)

Useful to resize imagefields.

```csharp
<img src="@Url.ImageWizard().Fetch(Model.Body).Resize(900,900).Grayscale().Blur().BuildUrl()">
```

## Docker 
[![Docker](https://img.shields.io/docker/pulls/usercode/imagewizard)](https://hub.docker.com/r/usercode/imagewizard)

```yml
static:
    image: usercode/imagewizard
    container_name: imagewizard
    restart: always
    networks:
      - default      
    volumes:
      - file_cache:/data
    environment:
      - General__Key=DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==
      - General__AllowUnsafeUrl=false
      - General__UseAcceptHeader=false
      - General__UseETag=true
      - General__AllowedDPR__0=1.0
      - General__AllowedDPR__1=1.5
      - General__AllowedDPR__2=2.0
      - General__AllowedDPR__3=3.0
      - General__CacheControl__IsEnabled=true
      - General__CacheControl__Public=true
      - General__CacheControl__MaxAge=60
      - General__CacheControl__MustRevalidate=false
      - General__CacheControl__NoCache=true
      - General__CacheControl__NoStore=true      
      - FileCache__Folder=/data
      - HttpLoader__DefaultBaseUrl=https://domain.tld
      - HttpLoader__AllowAbsoluteUrls=false
      - HttpLoader__AllowedHosts__0=domain.tld
      - HttpLoader__Headers__0__Name=ApiKey
      - HttpLoader__Headers__0__Value=123      
```
