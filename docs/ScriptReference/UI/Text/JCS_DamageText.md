# JCS_DamageText

Damage text on the mob.

## Variables

| Name                   | Description                                                                  |
|:-----------------------|:-----------------------------------------------------------------------------|
| mDamageTextEffectType  | Type of the damage text.                                                     |
| mMoveSpeed             | How fast the damage text moves.                                              |
| mSpacing               | Spacing between each digit.                                                  |
| mFadeSpeed             | Fade How fast the damage text fade out.                                      |
| mBaseOrderLayer        | Scene Layer in the render queue.                                             |
| mTimeType              | Type of the delta time.                                                      |
| mCapitalizeLetter      | The first letter will be bigger then other base on the scale variable below. |
| mCapitalLetterScale    | Scale of the first digit.                                                    |
| mWaveZiggeEffect       | Each digit will go up and down in order.                                     |
| mWaveZigge             | How much it does up and down.                                                |
| mAsymptoticScaleEffect | Do the asymptotic scale effect?                                              |
| mAsymptoticScale       | Scale value when doing the asymptotic scale effect.                          |
| mCritialSpriteScale    | Scale value to critical sprites.                                             |
| mSpacingX              | Spacing between each digit on x axis.                                        |
| mSpacingY              | Spacing between each digit on y axis.                                        |
| mRandomSize            | Randomize the size?                                                          |
| mMinSize               | Minimum size value.                                                          |
| mMaxSize               | Maximum size value.                                                          |

## Functions

| Name            | Description            |
|:----------------|:-----------------------|
| SpawnDamageText | Spawn one damage text. |
