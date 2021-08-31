# JCS_PfGrid

Path finding grid gameobject.

## Variables

| Name            | Description                           |
|:----------------|:--------------------------------------|
| mUnwalkableMask | Mask to detect the unwalkable object. |
| mGridiWorldSize | Size of the whole grid map."          |
| mNodeRadius     | Size of each grid.                    |
| mDirection      | Direction the grid approach to.       |

## Functions

| Name               | Description                           |
|:-------------------|:--------------------------------------|
| GetNeighbours      | Get the nodes around.                 |
| NodeFromWorldPoint | Find the node base on world position. |
