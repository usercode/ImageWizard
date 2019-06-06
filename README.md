# ImageWizard
A webservice to manipulate your images dynamically.

## Overview

Example:
https://localhost/fgj9fej98DFKG4eja/trim()/resize(200,200)/jpg(90)/https://upload.wikimedia.org/wikipedia/commons/b/b7/Europe_topography_map.png

Url parts:
- signature based on HMACSHA1
- any filters
- absolute url of the original image

## Available image filters

- resize(width,height)
- crop(x,y,width,height)
- grayscale()
- trim()

## Output formats

- jpg()
- jpg(quality)
- png()
- gif()
- bmp()
