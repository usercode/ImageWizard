# ImageWizard
A ASP.NET Core service / middleware to manipulate your images dynamically as alternative for thumbor.

| Package                       | Release | 
|--------------------------------|-----------------|
| ImageWizard.Core               | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/) |
| ImageWizard.MongoDB               | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.MongoDB.svg)](https://www.nuget.org/packages/ImageWizard.MongoDB/) |
| ImageWizard.AspNetCore (URL Builder)        | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.AspNetCore.svg)](https://www.nuget.org/packages/ImageWizard.AspNetCore/) |


## Overview

Example:
<p align="center">
https://localhost/  <br/>
image/  <br/>
WZy86ixQq9EogpyHwMYd7F5wKa0/  <br/>
resize(200,200)/grayscale()/jpg(90)/  <br/>
fetch/  <br/>
https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png
</p>

| Description         | Url segment |
|---------------------|-----------------|
| base path | "image" |
| signature based on HMACSHA256 | "WZy86ixQq9EogpyHwMYd7F5wKa0" or "unsafe" (if enabled) |
| any filters | "resize(200,200)/grayscale()/jpg(90)" |
| loader type | "fetch" |
| loader source | https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png | 

## Image filters
### Image transformations
- resize(size)
- resize(width,height)
- resize(width,height,mode)
  - mode: min, max, crop, pad, stretch
- crop(width,height)
- crop(x,y,width,height) //int for absolute values, 0.0 to 1.0 for relative values
- backgroundcolor(r,g,b)
  - int (0 to 255) or double (0.0 to 1.0)
- flip(type)
  - type: horizontal, vertical
- rotate(value) 
  - value: 90, 180 or 270
- grayscale()
- blackwhite()
- blur()
- invert()
- brightness(value)
- contrast(value)

### Output formats

- jpg()
- jpg(quality)
- png()
- gif()
- bmp()

### Special options
- dpr(value) //set allowed device pixel ratio
- nocache() //do not store the transformed image

## Image loaders
- HTTP loader ("fetch")
  - absolute or relative url of the original image
- file loader ("file")
  - relative url to file
- youtube loader ("youtube")
  - video id
- gravatar loader ("gravatar")

## Image caches

- file cache
- distributed cache
  - MS SQL
  - Redis
- MongoDB cache

## Integrate into existing ASP.NET Core applications

https://www.nuget.org/packages/ImageWizard.Core/


```csharp
services.AddImageWizard(); //use only http loader and distributed cache

//or

services.AddImageWizard(options => 
                       {
                           options.AllowUnsafeUrl = true;             
			   options.AllowedDPR = new[] { 1.0, 1.5, 2.0, 3.0, 4.0 };
                           options.Key = "DEMO-KEY..."; //64 byte key encoded in Base64Url
                           options.UseETag = true;
                           options.ImageMaxWidth = 4000;
                           options.ImageMaxHeight = 4000;                           
                           options.CacheControl.IsEnabled = true;
                           options.CacheControl.MaxAge = TimeSpan.FromDays(365);
                           options.CacheControl.MustRevalidate = false;
                           options.CacheControl.Public = true;
                           options.CacheControl.NoCache = false;
                           options.CacheControl.NoStore = false;
                       })
                       //use file cache
                       .SetFileCache(options => options.Folder = "FileCache")
                       //or MongoDB cache
                       .SetMongoDBCache(options => options.Hostname = "localhost")
                       //or distributed cache
                       .SetDistributedCache()
                       //add some loaders
                       .AddFileLoader(options => options.Folder = "FileStorage")
                       .AddHttpLoader(options => 
                                               //add custom http header like apikey to prevent 
                                               //that user can download the original image
                                               options.SetHeader("ApiKey", "123456")) 
                       .AddYoutubeLoader()
		                   .AddGravatarLoader()
		                   .AddInterceptor<MyInterceptor>()
                       ;
```

```csharp
app.UseImageWizard();
```

## Create custom filter

Simple example for resizing images:

- implements IFilter (or use base class 'FilterBase')
- add method with the name "Execute" and the parameters (supported types: string, int, double, enum)
- use DPR attribute for parameters which are depended on the device pixel ratio value
- add filter context parameter to get access to image and settings

```csharp
 public class ResizeFilter : FilterBase
    {
        public override string Name => "resize";

        public void Execute([DPR]int width, [DPR]int height, FilterContext context)
        {   
            context.Image.Mutate(m => m.Resize(width, height));
        }
    }
```

Register filter:
```csharp
services.AddImageWizard()
	.AddFilter<ResizeFilter>();
```
URL segment: 
```csharp
"/resize(200,100)/"
```

## ASP.NET Core UrlBuilder

https://www.nuget.org/packages/ImageWizard.AspNetCore/

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
});
```

Create url with fluent api

```csharp
@Url
.ImageWizard()
//use HTTP loader
.Fetch("https://<your-domain>/test/picture.jpg")
//or file loader
.File("test/picture.jpg")
.Trim()
.Resize(160,140)
.Jpg(90)
.BuildUrl()
```

