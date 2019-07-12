# ImageWizard
A webservice to manipulate your images dynamically.

| Package                       | Release | 
|--------------------------------|-----------------|
| ImageWizard.Core               | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.Core.svg)](https://www.nuget.org/packages/ImageWizard.Core/) |
| ImageWizard.MongoDB               | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.MongoDB.svg)](https://www.nuget.org/packages/ImageWizard.MongoDB/) |
| ImageWizard.AspNetCore               | [![NuGet](https://img.shields.io/nuget/v/ImageWizard.AspNetCore.svg)](https://www.nuget.org/packages/ImageWizard.AspNetCore/) |


## Overview

Example:

https://localhost/image/WZy86ixQq9EogpyHwMYd7F5wKa0/trim()/resize(200,200)/jpg(90)/fetch/https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png

| Description         | Url segment |
|---------------------|-----------------|
| base path | "image" |
| signature based on HMACSHA1 | "WZy86ixQq9EogpyHwMYd7F5wKa0" or "unsafe" (if enabled) |
| any filters | "trim()/resize(200,200)/jpg(90)" |
| delivery type | "fetch" or "upload" |
| absolute url of the original image | https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png | 

## Image filters
### Image transformations
- resize(size)
- resize(width,height)
- resize(width,height,mode)
  - mode: min, max, crop, pad, stretch
- crop(width,height)
- crop(x,y,width,height) //int for absolute values, 0.0 to 1.0 for relative values
- flip(type)
  - type: horizontal, vertical
- rotate(value) 
  - value: 90, 180 or 270
- trim()
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

## Image loaders
- HTTP loader ("fetch")
  - absolute url of the original image
- file loader ("upload")
  - relative url to file

## Image caches

- file cache
- distributed cache
  - MS SQL
  - Redis
- MongoDB cache

## Integrate into existing ASP.NET Core applications

https://www.nuget.org/packages/ImageWizard.Core/


```csharp
services.AddImageWizard(options => 
                       {
                           options.AllowUnsafeUrl = true;                           
                           options.Key = "DEMO-KEY...";
                           options.UseETag = true;
                           options.ResponseCacheControlMaxAge = TimeSpan.FromDays(90);
                       })
                       //use file cache
                       .SetFileCache(options => options.Folder = "FileCache")
                       //or MongoDB cache
                       .SetMongoDBCache(options => options.Hostname = "localhost")
                       //or distributed cache
                       .SetDistributedCache()
                       .AddHttpLoader()
                       .AddFileLoader(options => options.Folder = "FileStorage");
```

```csharp
app.UseImageWizard();
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
services.Configure<ImageWizardSettings>(Configuration.GetSection("ImageWizard"));
```

Create url with fluent api

```csharp
@Url
.ImageWizard()
//use HTTP loader
.Fetch(Url.RouteUrl("MyImage", new { mediaUrl = Model.MediaUrl }, Context.Request.Scheme))
//or file loader
.Upload("/test/bild.jpg")
.Trim()
.Resize(160,140)
.Jpg(90)
.BuildUrl()
```

