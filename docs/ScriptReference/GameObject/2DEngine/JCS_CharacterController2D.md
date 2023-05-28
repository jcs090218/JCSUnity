# JCS_CharacterController2D

Character Controller using Unity 2D Engine.

## Variables

| Name                       | Description                                     |
|:---------------------------|:------------------------------------------------|
| mApplyGravity              | Apply gravity?                                  |
| mJumpForce                 | Impetus for this object to jump.                |
| mDetectDistance            | How far the raycast is cast.                    |
| mDeltaTimeType             | Type of the delta time.                         |
| mFreezeX                   | Freeze the object in x axis.                    |
| mFreezeY                   | Freeze the object in y axis.                    |
| mZeroRotationWhenIsTrigger | Zero out the rotation when collider is trigger. |

## Functions

| Name       | Description                |
|:-----------|:---------------------------|
| Jump       | Jump once.                 |
| isGrounded | Check if is on the ground. |
