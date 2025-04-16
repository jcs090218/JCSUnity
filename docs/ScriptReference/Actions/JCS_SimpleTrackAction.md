# JCS_SimpleTrackAction

The action that moves toward a position.

## Variables

| Name           | Description                                            |
|:---------------|:-------------------------------------------------------|
| mTarget        | Target transform; if null use target position instead. |
| mTargetPos     | Target position to track.                              |
| mFriction      | How fast it moves toward to the target position?       |
| mTimeType      | Type of the delta time.                                |
| mIgnoreX       | Don't track on x-axis?                                 |
| mIgnoreY       | Don't track on y-axis?                                 |
| mIgnoreZ       | Don't track on z-axis?                                 |
| mLocalTarget   | Use local variables for target instead.                |
| mLocalSelf     | Use local variables for self instead.                  |

## Functions

| Name            | Description                                        |
|:----------------|:---------------------------------------------------|
| DeltaTargetPosX | Change the target position on x axis by adding it. |
| DeltaTargetPosY | Change the target position on y axis by adding it. |
| DeltaTargetPosZ | Change the target position on z axis by adding it. |
