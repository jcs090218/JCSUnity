# JCS_SoundPlayer

Sound player for any component that need to play sound effect.

## Variables

| Name             | Description                               |
|:-----------------|:------------------------------------------|
| SoundSettingType | Sound setting type for this sound player. |

## Functions

| Name                       | Description                                                                                      |
|:---------------------------|:-------------------------------------------------------------------------------------------------|
| PlayByAttachment           | Play AC by method depends on the attachement of the SP.                                          |
| PlayOneShot                | Play one shot of sound.                                                                          |
| PlayOneShotWhileNotPlaying | Play the sound while no sound is playing.                                                        |
| PlayOneShotInterrupt       | Interrupt the all the sound from the sound player, and play the current sound effect.            |
| PlayOneShotByMethod        | Play one shot depedings on the methods. (See: [JCS_SoundMethod](?page=Enums_sl_JCS_SoundMethod)) |
