# JCS_VideoLoader

Video loader, load an external video file.

## Functions

| Name      | Description                  |
|:----------|:-----------------------------|
| LoadVideo | Load an external video file. |

## Example

```cs
VideoPlayer vp = GetComponent<VideoPlayer>();

string filePath = "path/to/video/file.mp4";

JCS_VideoLoader.LoadVideo(vp, filePath);
```
