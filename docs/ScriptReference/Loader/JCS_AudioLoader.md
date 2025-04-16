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
      string url = "file://C:/path/to/file.ogg";
  
      // Start loading the audio file.
      StartCoroutine(JCS_AudioLoader.LoadAudio(url, AudioLoaded));
  }

  /// <summary>
  /// Callback after the audio is loaded.
  /// </summary>
  private void AudioLoaded(AudioClip ac)
  {
      Debug.Log("Done loading the audio file!");
  }
```
