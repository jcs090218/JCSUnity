# JCS_SoundSettings

Sound related settings.

## Variables

| Name                    | Description                                                   |
|:------------------------|:--------------------------------------------------------------|
| mixer                   | The current audio mixer.                                      |
| mTimeOut                | Time to fade out the background music.                        |
| mTimeIn                 | Time to fade in the background music.                         |
| keepBGMSwitchScene      | Keep the background music when switch to next scene.          |
| smoothSwithBetweenScene | Smoothly switching the sound between the switching the scene. |
| clipBGM                 | Clip that will play as background music for this scene.       |
| clipWindowOpen          | Sound to play when open the window clip.                      |
| clipWindowClose         | Sound to play when close the window clip.                     |

## Functions

| Name    | Description                                                                  |
|:--------|:-----------------------------------------------------------------------------|
| TimeOut | Get the real sound fade out time base on the sound manager override setting. |
| TimeIn  | Get the real sound fade in time base on the sound manager override setting.  |
