# JCS_Audio

Audio utilities.

## Functions

| Name            | Description                                                                                   |
|:----------------|:----------------------------------------------------------------------------------------------|
| GetFloat        | Return a float value from the audio mixer.                                                    |
| Volume2Decibel  | Convert volume level to decibel.                                                              |
| Decibel2Volume  | Convert decibel to volume level.                                                              |
| GetVolume       | Return the volume level from the audio mixer.                                                 |
| SetVolume       | Set the audio mixer's volume.                                                                 |
| PlayClipAtPoint | Same with function `AudioSource.PlayClipAtPoint` with different default `spatialBlend` value. |
| DestroyClip     | Destroy the clip by its clip length.                                                          |
