# JCS_2DWalkAction

Action does the AI walk action in 2D.

## Variables

| Name                  | Description                                                          |
|:----------------------|:---------------------------------------------------------------------|
| mWalkSpeed            | Speed of the action.                                                 |
| mToLeft               | Possibility to walk LEFT way.                                        |
| mToRight              | Possibility to walk RIGHT way.                                       |
| mToIdle               | Possibility to IDLE.                                                 |
| mPossibility          | Possiblity to active this action.                                    |
| mTimeZone             | Time to do one walk.                                                 |
| mAdjustTimeZone       | Time that will randomize the Time Zone value.                        |
| mTimeType             | Type of the delta time.                                              |
| mStartRandomWalkSpeed | Generate a random walk speed at the initilaize time.                 |
| mRandomWalkSpeedRange | Addition value to the walk speed.                                    |
| mMadEffect            | If get mad will start tracking the object that make this object mad. |
| mAttackRecorder       | Recorder records the attacker.                                       |
| mLiveObjectAnimator   | Live object animation.                                               |

## Functions

| Name                     | Description                                                     |
|:-------------------------|:----------------------------------------------------------------|
| WalkByPossiblity         | Do the walk action depends on possibility.                      |
| WalkDirectionPossibility | Recusive function that will find the direction and do the walk. |
| WalkRandomly             | Walk randomly.                                                  |
| WalkByStatus             | Walk depends on the status.                                     |
| Walk                     | Do the walk action.                                             |
