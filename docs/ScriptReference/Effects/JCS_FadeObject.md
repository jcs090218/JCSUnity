# JCS_FadeObject

Fade object to a particular alpha channel.

## Variables

| Name            | Description                                        |
|:----------------|:---------------------------------------------------|
| onAfterFade     | Execution after its done fading.                   |
| onAfterFadeIn   | Execution after its done fading in.                |
| onAfterFading   | Execution after its done fading out.               |
| onBeforeFade    | Exectuion before start fading.                     |
| onBeforeFadeIn  | Exectuion before start fading in.                  |
| onBeforeFadeOut | Exectuion before start fading out.                 |
| onFading        | Execution while fading.                            |
| mEffect         | Is current fade object doing the effect?           |
| mVisible        | Is current fade object visible?                    |
| mFadeTime       | How long it fades.                                 |
| mOverrideFade   | Override the action before it complete the action. |
| mFadeInAmount   | Maxinum of fade value.                             |
| mFadeOutAmount  | Mininum of fade value.                             |
| mTimeType       | Type of the delta time.                            |

## Functions

| Name      | Description                      |
|:----------|:---------------------------------|
| IsFadeIn  | Check if the object is fade in?  |
| IsFadeOut | Check if the object is fade out. |
| FadeOut   | Fade out the object.             |
| FadeIn    | Fade in the object.              |
