# JCS_SoundManager

Manage of all the music, sound and SFX in the game.

## Variables

| Name                           | Description                                |
|:-------------------------------|:-------------------------------------------|
| mOverrideSetting               | Do this scene using the specific setting?  |
| mSoundFadeInTime               | Time to fade in the sound.                 |
| mSoundFadeOutTime              | Time to fade out the sound.                |
| mDisableSoundWheWindowNotFocus | Disable the sound when window isn't focus. |

## Functions

| Name                       | Description                                          |
|:---------------------------|:-----------------------------------------------------|
| SwitchBackgroundMusic      | Switch the background music, fading in and out.      |
| PlayOneShotBackgroundMusic | Play one shot background music, after playing it.    |
| PlayOneShotSFXSound        | Push to the sound effect into array ready for use!   |
| SetSoundVolume             | Set the sound volume base on type.                   |
| SetSoundMute               | Set weather the sound are mute or not by sound type. |
