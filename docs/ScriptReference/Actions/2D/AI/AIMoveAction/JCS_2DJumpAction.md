# JCS_2DJumpAction

Action does the AI jump action in 2D.

## Variables

| Name                  | Description                                          |
|:----------------------|:-----------------------------------------------------|
| mJumpForce            | How much force to do one jump.                       |
| mPossibility          | Possiblity to active this action.                    |
| mTimeZone             | Time to do one jump.                                 |
| mAdjustTimeZone       | Time that will randomize the Time Zone value.        |
| mStartRandomJumpForce | Generate a random jump force at the initilaize time. |
| mRandomJumpForceRange | Addition value to the jump force.                    |
| mLiveObjectAnimator   | Live object animation.                               |

## Functions

| Name              | Description                 |
|:------------------|:----------------------------|
| JumpByPossibility | Do the jump by possibility. |
| Jump              | Do the jump action.         |
