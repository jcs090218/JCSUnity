# JCS_3DShakeEffect

Effect that shake the transform in 3D.

## Variables

| Name           | Description                                           |
|:---------------|:------------------------------------------------------|
| onBeforeShake  | Callback executed before the shake effect.            |
| onShake        | Callback executed while doing the shake effect.       |
| onAfterShake   | Callback executed after the shake effect.             |
| TransformType  | Shake on this transform properties.                   |
| Force          | Force the effect even when its already in the motion. |
| Time           | How long it shakes.                                   |
| eMargin        | How intense it shakes.                                |
| mTimeType      | Type of the delta time.                               |
| OnX            | Do shake on x axis.                                   |
| OnY            | Do shake on y axis.                                   |
| OnZ            | Do shake on z axis.                                   |
| SoundPlayer    | Sound player for 3D sounds calculation.               |
| Clip           | Sound played when effect occurs.                      |

## Functions

| Name    | Description          |
|:--------|:---------------------|
| DoShake | Do the shake effect. |
