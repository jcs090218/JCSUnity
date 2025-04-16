# JCS_StreamAssets

Generic streaming assets interface.

## Variables

| Name        | Description                    |
|:------------|:-------------------------------|
| preloadPath | Preload streaming assets path. |

## Functions

| Name                | Description                                     |
|:--------------------|:------------------------------------------------|
| StreamingAssetsPath | Streaming assets path.                          |
| CachePath           | Return the streaming assets cache path.         |
| UrlPath             | Return static URL path.                         |
| NeedRequestData     | Check if current data are requesting data.      |
| ReadAllBytes        | Get all the bytes by PATH.                      |
| AddDownloadTarget   | Add a download target by streaming assets PATH. |
