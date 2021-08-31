# JCS_ImageLoader

Image loader, load an external image file to Unity usable sprite.

## Functions

| Name        | Description                 |
|:------------|:----------------------------|
| LoadTexture | Load image file as texture. |
| Create      | Create sprite object.       |
| LoadImage   | Load Image by file path.    |

## Example

```cs
string filePath = "path/to/image.png";

// Get the image as texture.
Texture2D tex = JCS_ImageLoader.LoadTexture(filePath);

// Convert texture to sprite.
Sprite sp = JCS_ImageLoader.Create(tex);

// Combine the two functions just use.
sp = JCS_ImageLoader.LoadImage(filePath);
```
