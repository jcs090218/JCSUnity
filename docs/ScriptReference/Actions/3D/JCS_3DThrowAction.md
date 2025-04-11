# JCS_3DThrowAction

Throws a game object toward another game object.

## Variables

| Name            | Description                         |
|:----------------|:------------------------------------|
| mActive         | Is this component active?           |
| mGravityProduct | Mulitply the gravity.               |
| mForce          | Force to hit the target.            |
| mTime           | Target time to hit the target.      |
| mTimeType       | Type of the delta time.             |
| mFaceFoward     | Rotate to look at forward location. |

## Functions

| Name         | Description                                     |
|:-------------|:------------------------------------------------|
| ThrowByTime  | Do the throw action by time.                    |
| ThrowByForce | Do the throw action by calculate the kinematic. |
