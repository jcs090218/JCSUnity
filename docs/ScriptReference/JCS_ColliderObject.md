# JCS_ColliderObject

Contain all possible collider component and information.

## Variables

| Name                | Description                             |
|:--------------------|:----------------------------------------|
| ColliderType        | Type of the current collider.           |
| characterController | Collider `character controller` object. |
| boxCollider         | Collider `box collider` object.         |
| sphereCollider      | Collider `sphere collider` object.      |
| capsuleCollider     | Collider `capsule collider` object.     |
| boxCollider2D       | Collider `box collider 2D` object.      |
| circleCollider2D    | Collider `circle collider 2D` object.   |
| capsuleCollider2D   | Collider `capsule collider 2D` object.  |
| center              | Shared local center of collider type.   |
| offset              | Shared local offset of collider type.   |
| size                | Shared local size of collider type.     |
| radius              | Shared local radius of collider type.   |
| height              | Shared height size of collider type.    |

## Functions

| Name               | Description                              |
|:-------------------|:-----------------------------------------|
| DetectColliderOnce | Identify the current collider type once. |
| IsColliderType     | Check if TYPE current collider type.     |
