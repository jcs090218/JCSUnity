# JCS_2DFlyAction

Action does the AI fly action on 2D.

## Variables

| Name                | Description                                                               |
|:--------------------|:--------------------------------------------------------------------------|
| mFlyForceX          | Speed of flying on x-axis.                                                |
| mFlyForceY          | Speed of flying on y-axis.                                                |
| mToUp               | Possiblity of going UP.                                                   |
| mToDown             | Possiblity of going DOWN.                                                 |
| mToLeft             | Possiblity of going LEFT.                                                 |
| mToRight            | Possiblity of going RIGHT.                                                |
| mToIdleHorizontal   | Possibility to IDLE in horizontal direction.                              |
| mToIdleVetical      | Possibility to IDLE in vertical direction.                                |
| mPossibility        | Possiblity to active this action.                                         |
| mTimeZone           | Time to do one Fly.                                                       |
| mAdjustTimeZone     | Time that will randomize the Time Zone value.                             |
| mDeltaTimeType      | Type of the delta time.                                                   |
| mMinHeight          | Lowest height the object can go.                                          |
| mMaxHeight          | Highest height the object can go.                                         |
| mMadEffect          | If get mad will start tracking the object that make this object mad.      |
| mAttackRecorder     | Recorder records the attacker.                                            |
| mLiveObjectAnimator | Live object animation.                                                    |
| mIgnorePlatform     | Check this to make the object ignore all the platform at initialize time. |
| mFlySound           | Sound while flying.                                                       |
| mSoundPlayer        | Sound player to play sounds.                                              |

## Functions

| Name                     | Description                                            |
|:-------------------------|:-------------------------------------------------------|
| FlyByPossiblity          | Calculate the possiblity and see if do the fly action. |
| FlyDirectionByPossiblity | Fly to a direction base on possibilities.              |
| FlyRandomly              | Fly randomly.                                          |
| FlyByStatus              | Process velocity and animation by passing the status.  |
| FlyX                     | Fly on x-axis.                                         |
| FlyY                     | Fly on y-axis.                                         |
