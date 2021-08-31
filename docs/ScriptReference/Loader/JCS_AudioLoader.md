# JCS_AudioLoader

Audio loader, load an external audio file.

## Functions

| Name      | Description                              |
|:----------|:-----------------------------------------|
| LoadAudio | Load the music from the path in runtime. |

## Example

```cs
  private void Start()
  {
      string filePath = "path/to/file.ogg";
      AudioClip cl;
  
      // Start loading the audio file.
      StartCoroutine(JCS_AudioLoader.LoadAudio(cl, filePath, DoneLoadMusic));
  }

  /// <summary>
  /// Callback after the audio is loaded.
  /// </summary>
  private void DoneLoadMusic(AudioClip ac)
  {
      Debug.Log("Done loading the audio file!");
  }
```
