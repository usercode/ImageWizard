# ImageWizard
A webservice to manipulate your images dynamically.

## Overview

Example:

https://localhost/image/WZy86ixQq9EogpyHwMYd7F5wKa0/trim()/resize(200,200)/jpg(90)/fetch/https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png

unsafe version (if enabled):
https://localhost/image/unsafe/trim()/resize(200,200)/jpg(90)/fetch/https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png

Url parts:
- base path ("/image")
- signature based on HMACSHA1 and encoded in Base64Url
- any filters
- delivery type: fetch
- absolute url of the original image

## Integrate into existing ASP.NET Core applications

https://www.nuget.org/packages/ImageWizard.Core/

```csharp
services.AddImageWizard(options => 
                       {
                           options.AllowUnsafeUrl = true;
                           options.Key = "DEMO-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---PLEASE-CHANGE-THIS-KEY---==";
                           options.ResponseCacheTime = TimeSpan.FromDays(90);
                       })
                       .AddDefaultFilters()
                       .AddFileCache(options => options.RootFolder = env.WebRootPath)
                       .AddHttpLoader();
```

```csharp
app.UseImageWizard();
```

## Available image filters

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

## Output formats

- jpg()
- jpg(quality)
- png()
- gif()
- bmp()

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
.ImageWizard(Url.RouteUrl("MyImage", new { mediaUrl = Model.MediaUrl }, Context.Request.Scheme))
.Trim()
.Resize(160,140)
.Jpg(90)
.BuildUrl()
```

