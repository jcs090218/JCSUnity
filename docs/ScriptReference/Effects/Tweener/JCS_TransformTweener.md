# JCS_TransformTweener

Transform tweener.

## Variables

| Name                      | Description                                               |
|:--------------------------|:----------------------------------------------------------|
| onStart                   | Callback to execute when start tweening.                  |
| onDone                    | Callback to execute when done tweening.                   |
| mTween                    | Do the tween effect?                                      |
| mValueOffset              | Value offset.                                             |
| mDurationX                | How fast it moves on x axis.                              |
| mDurationY                | How fast it moves on y axis.                              |
| mDurationZ                | How fast it moves on z axis.                              |
| mTimeType                 | Type of the delta time.                                   |
| mDestroyWhenDoneTweening  | Destroy this object when done tweening?"                  |
| mDestroyDoneTweeningCount | How many times of done tweening destroy will active?      |
| mRandomizeDuration        | Randomize the durations with all axis at start. (x, y, z) |
| mTweenType                | Which transform's properties to tween.                    |
| mTrackAsLocalSelf         | Change the self position as local position.               |
| mTrackAsLocalTarget       | Track the target as local position.                       |
| mEasingX                  | Tweener formula on x axis.                                |
| mEasingY                  | Tweener formula on y axis.                                |
| mEasingZ                  | Tweener formula on z axis.                                |
| mStopTweenDistance        | While continue tween when did the tweener algorithm stop? |

## Functions

| Name                          | Description                                                       |
|:------------------------------|:------------------------------------------------------------------|
| ResetTweener                  | Reset tweener effect setting.                                     |
| DoTween                       | Tween to this vector either position, scale, rotation.            |
| DoTweenContinue               | Continue Tween to this target's either position, scale, rotation. |
| GetSelfTransformTypeVector3   | Get itself transform type's vector3 value.                        |
| SetSelfTransformTypeVector3   | Set self transform value.                                         |
| GetTargetTransformTypeVector3 | Get target transform type's vector3 value.                        |
