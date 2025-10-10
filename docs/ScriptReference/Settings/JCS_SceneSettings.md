# JCS_SceneSettings

Handle the scene the better way.

## Variables

| Name        | Description                          |
|:------------|:-------------------------------------|
| mTimeIn     | Scene fade out time.                 |
| mTimeOut    | Scene fade in time.                  |
| screenColor | Screen color to do fade in/out.      |
| mClipIn     | The video clip to play for fade in.  |
| mClipOut    | The video clip to play for fade out. |

## Functions

| Name    | Description                                                  |
|:--------|:-------------------------------------------------------------|
| TimeOut | Return the time for fade out the scene base on the settings. |
| TimeIn  | Return the time for fade in the scene base on the settings.  |
| ClipIn  | Return the video clip for fading in.                         |
| ClipOut | Return the video clip for fading out.                        |
