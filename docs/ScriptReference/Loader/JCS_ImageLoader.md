# JCS_ImageLoader

Image loader, load an external image file to Unity usable sprite.

## Functions

| Name        | Description                 |
|:------------|:----------------------------|
| LoadTexture | Load image file as texture. |
| Create      | Create sprite object.       |
| LoadImage   | Load Image by file path.    |

## Example

You can either loaded through local file system,

```cs
string filePath = "path/to/image.png";

// Get the image as texture.
Texture2D tex = JCS_ImageLoader.LoadTexture(filePath);

// Convert texture to sprite.
Sprite sp = JCS_ImageLoader.Create(tex);

// Combine the two functions just use.
sp = JCS_ImageLoader.LoadImage(filePath);
```

Or loaded from an URL using web request!

```cs
  private void Start()
  {
      string url = "file://C:/path/to/image.png";  // you can use HTTP url instead
  
      // Start loading the image file.
      StartCoroutine(JCS_ImageLoader.LoadImage(url, ImageLoaded));
  }

  /// <summary>
  /// Callback after the image is loaded.
  /// </summary>
  private void ImageLoaded(Texture2D tex)
  {
      Debug.Log("Done loading the image file!");
  }
```
