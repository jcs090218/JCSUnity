# JCS_3DWalkAction

Simulate the walk action in 3D space.

## Variables

| Name                    | Description                                                                |
|:------------------------|:---------------------------------------------------------------------------|
| Active                  | Check weather you want do this action.                                     |
| WalkType                | Type of the walk behaviour calculation.                                    |
| SearchCount             | Count for how many search per frame.                                       |
| AllowOverlapDestination | Allow each walk action having the same destination.                        |
| OverlapDistance         | Distance that would count as overlap destination.                          |
| AcceptRemainDistance    | What value count as path complete action.                                  |
| MinOffDistance          | Minimum randomly add vector with magnitude of distance at target position. |
| MaxOffDistance          | Maximum randomly add vector with magnitude of distance at target position. |
| SelfDistance            | Self distance without target transform interact.                           |
| RangeDistance           | Range that enemy will try to get close to.                                 |
| AdjustRangeDistance     | Randomly adjusts the range distance.                                       |

## Functions

| Name            | Description                                          |
|:----------------|:-----------------------------------------------------|
| AgentNow        | Turn the current nav mesh to agent.                  |
| ObstacleNow     | Turn the current nav mesh to obstacle.               |
| IsAgent         | Return true if current action is agent.              |
| IsObstacle      | Return true if current action is obstacle.           |
| TargetOne       | Target one player and do in taget action.            |
| NavMeshArrive   | Check if the nav mesh agent arrive the destination.  |
| IsArrived       | Check if nav mesh agent path completed.              |
| InRangeDistance | Check if the transform in the range of the distance. |
