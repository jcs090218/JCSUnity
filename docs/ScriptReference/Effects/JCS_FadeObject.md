# JCS_FadeObject

Fade object to a particular alpha channel.

## Variables

| Name           | Description                                        |
|:---------------|:---------------------------------------------------|
| onFadeOut      | The callback executes after fade out.              |
| onFadeIn       | The callback executes after fade-in.               |
| onFading       | The callback executes when fading in/out.          |
| mEffect        | Is current fade object doing the effect?           |
| mVisible       | Is current fade object visible?                    |
| mFadeTime      | How long it fades.                                 |
| mOverrideFade  | Override the action before it complete the action. |
| mFadeInAmount  | Maxinum of fade value.                             |
| mFadeOutAmount | Mininum of fade value.                             |
| mDeltaTimeType | Type of the delta time.                            |

## Functions

| Name      | Description                      |
|:----------|:---------------------------------|
| IsFadeIn  | Check if the object is fade in?  |
| IsFadeOut | Check if the object is fade out. |
| FadeOut   | Fade out the object.             |
| FadeIn    | Fade in the object.              |
