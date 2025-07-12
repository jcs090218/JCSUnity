# JCS_3DCamera

Basic camera in 3D game.

## Variables

| Name                 | Description                                |
|:---------------------|:-------------------------------------------|
| mTargetTransform     | Target we want to look at.                 |
| mTargetRevolution    | Targeting revolution.                      |
| mSmoothTrackFriction | How fast the camera track in each axis?    |
| mRotateFriction      | How fast this camera rotates.              |
| mRotateAngle         | Angle that rotate once.                    |
| mResetTargetAngle    | Angle when reset the camera.               |
| mRotateAroundLeft    | Key to rotate around the left side.        |
| mRotateAroundRight   | Key to rotate around the right side.       |
| mResetKeyCode        | Key to reset the camera.                   |
| mResetTargetAngle    | Angle when reset the camera.               |
| mUpDownMovement      | Up and Down movement enable?               |
| mUpKey               | Key to go up.                              |
| mDownKey             | Key to go down.                            |
| mUpDownSpacing       | Space between each up and down movement.   |
| mMaxHeight           | How high the camera can reach.             |
| mMinHeight           | How low the camera can reach.              |
| mUpDownFriction      | How fast it change the view up and down?   |
| mZoomEffect          | Do the zoom effect?                        |
| ScrollRange_Mouse    | Distance once you scroll with mouse.       |
| ScrollRange_Touch    | Distance once you scroll with multi-touch. |
| mScrollSpeedFriction | How fast it scroll speed get reduce?       |
| MinDistance          | Mininum distance camera can approach to?   |
| MaxDistance          | Maxinum distance camera can far away from? |

## Functions

| Name              | Description                            |
|:------------------|:---------------------------------------|
| RotateAroundRight | Rotate around the target toward right. |
| RotateAroundLeft  | Rotate around the target toward left.  |
| ResetCamera       | Reset the camera.                      |
| UpCamera          | Move the camera upward.                |
| DownCamera        | Move the caemra downward.              |
