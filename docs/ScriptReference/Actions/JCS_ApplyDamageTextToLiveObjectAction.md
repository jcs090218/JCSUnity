# JCS_ApplyDamageTextToLiveObjectAction

Apply the damage to live object by automatically.

## Variables

| Name                      | Description                                                                            |
|:--------------------------|:---------------------------------------------------------------------------------------|
| mAbilityFormat            | Ability format for calculation.                                                        |
| mDamageTextPositionOffset | Position offset where damage text spawns.                                              |
| mPreCalculateEffect       | Attack will be calculate before hit the object.                                        |
| mDamageApplying           | Damages that store in here ready to apply to the target.                               |
| mOnlyWithTarget           | Lock on the target?                                                                    |
| mTargetTransform          | Target we lock on!                                                                     |
| mRandPos                  | Random position effect.                                                                |
| mRandPosRange             | Random position limit to within this range.Random position limit to within this range. |
| mDestroyByThisAction      | Destroy live object by this object.                                                    |
| mIsAOE                    | Make object un-destroyable, count down by AOECount below.                              |
| mAOECount                 | Once the object hit a object count one down.                                           |
| mHitSound                 | Play this hit sound, while is action happens.                                          |

## Functions

| Name       | Description                           |
|:-----------|:--------------------------------------|
| CopyToThis | Copy some information from the other. |
