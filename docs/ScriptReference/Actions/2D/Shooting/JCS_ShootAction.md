# JCS_ShootAction

Action that shoot bullets.

## Variables

| Name                    | Description                                                                                      |
|:------------------------|:-------------------------------------------------------------------------------------------------|
| mBullet                 | Bullet to use.                                                                                   |
| mSpawnPoint             | Spawn point.                                                                                     |
| mSpanwPointOffset       | Offset position to spawn point.                                                                  |
| mDefaultHit             | Default hit active to live object.                                                               |
| mShootCount             | How many bullet everytime active shoot event.                                                    |
| mShootKeyCode           | Shoot keycode.                                                                                   |
| mMouseButton            | Mouse button to shoot.                                                                           |
| mKeyAct                 | Key action type.                                                                                 |
| mTimeBeforeShoot        | Delay time before shooting a bullet.                                                             |
| mTimeDelayAfterShoot    | Delay time after shooting a bullet.                                                              |
| mAutoShoot              | Shoot the bullet depend on the delay time.                                                       |
| mBulletSpeed            | Speed apply to the bullet                                                                        |
| mDelayTime              | How long it take to shoot a bullet.                                                              |
| mDeviationEffectX       | Deviate the angle on x-axis.                                                                     |
| mDeviationRangeX        | Deviate range on x-axis.                                                                         |
| mDeviationEffectY       | Deviate the angle on y-axis.                                                                     |
| mDeviationRangeY        | Deviate range on y-axis.                                                                         |
| mDeviationEffectZ       | Deviate the angle on z-axis.                                                                     |
| mDeviationRangeZ        | Deviate range on z-axis.                                                                         |
| mRandPosX               | Spawn bullet at random position on x-axis.                                                       |
| mRandPosRangeX          | Random position apply on x-axis.                                                                 |
| mRandPosY               | Spawn bullet at random position on y-axis.                                                       |
| mRandPosRangeY          | Random position apply on y-axis.                                                                 |
| mRandPosZ               | Spawn bullet at random position on z-axis.                                                       |
| mRandPosRangeZ          | Random position apply on z-axis.                                                                 |
| mRandomMultiSoundAction | Sound when shoot action occurs.                                                                  |
| mAbilityFormat          | How much damage apply to other objects.                                                          |
| mTrackSoot              | Will shoot to the target depends on Detect Area Action.                                          |
| mDetectAreaAction       | Physical area to detect the [JCS_DetectAreaObject](?page=Actions_sl_JCS_DetectAreaObject). (tag) |
| mTrackType              | Track type.                                                                                      |
| mPlayer                 | Player uses the shoot action.                                                                    |

## Functions

| Name                | Description                            |
|:--------------------|:---------------------------------------|
| Shoot               | Shoot a bullet.                        |
| ShootWithShootCount | Shoot the bullet with the shoot count. |
