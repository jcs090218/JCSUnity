# JCS_OneJump

The effect makes the item jumps and spreads.

## Variables

| Name                | Description                                                                  |
|:--------------------|:-----------------------------------------------------------------------------|
| mDeltaTimeType      | Type of the delta time.                                                      |
| mJumpForce          | How many force to apply on jump?                                             |
| mMoveForce          | How fast this item moves?                                                    |
| mItemGravity        | Item gravity.                                                                |
| mBounceBackfromWall | Do the item bounce back from the wall after hit the wall or just stop there. |
| mBounceFriction     | Deacceleration after bouncing from the wall.                                 |

## Functions

| Name    | Description                            |
|:--------|:---------------------------------------|
| DoForce | Apply force in order to do hop effect. |
