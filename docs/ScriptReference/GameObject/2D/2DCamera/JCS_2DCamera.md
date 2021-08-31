# JCS_2DCamera

Basic camera for 2d Games.

## Variables

| Name                               | Description                                                   |
|:-----------------------------------|:--------------------------------------------------------------|
| mSetToPlayerPositionAtStart        | Set the camera's position to the player when the game starts? |
| mTargetTransform                   | Target transform information.                                 |
| mResetVelocityToZeroWhileNotActive | Set velocity to zero while the follow not active.             |
| mFrictionX                         | How fast this camera move toward the target. (x-axis)         |
| mFrictionY                         | How fast this camera move toward the target. (y-axis)         |
| mFreezeInRuntime                   | Do freeze in runtime?                                         |
| mFreezeX                           | Freeze on X axis?                                             |
| mFreezeY                           | Freeze on Y axis?                                             |
| mFreezeZ                           | Freeze on Z axis?                                             |
| mZoomEffect                        | Do the zoom effect?                                           |
| ZoomWithMouseOrTouch               | Zoom with the mouse or touches.                               |
| ScrollRange_Mouse                  | Distance once you scroll with mouse.                          |
| ScrollRange_Touch                  | Distance once you scroll with multi-touch.                    |
| mScrollFriction                    | How fast it scroll. (Zoom In/Out)                             |
| mMax_X_PositionInScene             | Maxinum this camera can go in x-axis.                         |
| mMin_X_PositionInScene             | Mininum this camera can go in x-axis.                         |
| mMax_Y_PositionInScene             | Maxinum this camera can go in y-axis.                         |
| mMin_Y_PositionInScene             | ininum this camera can go in y-axis.                          |

## Functions

| Name                 | Description                            |
|:---------------------|:---------------------------------------|
| SetToTargetImmediate | Set to the target's position in frame. |
| ZoomCamera           | Do the zooming on Z axis.              |
