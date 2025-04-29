# JCS_3DThrowAction

Throws a game object toward another game object.

## Variables

| Name            | Description                         |
|:----------------|:------------------------------------|
| mActive         | Is this component active?           |
| mGravityProduct | Mulitply the gravity.               |
| mTimeType       | Type of the delta time.             |
| mFaceFoward     | Rotate to look at forward location. |
| mForce          | Force to hit the target.            |
| mDegree         | Angle degree to hit the target.     |
| mTime           | Target time to hit the target.      |

## Functions

| Name           | Description                                     |
|:---------------|:------------------------------------------------|
| ThrowByTime    | Do the throw action by time.                    |
| ThrowByForce   | Do the throw action by calculate the kinematic. |
| GetArchByTime  | Return a list of arch positions.                |
| GetArchByForce | Return a list of arch positions.                |
