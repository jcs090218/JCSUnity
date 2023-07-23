# JCS_3DDistanceTileAction

Move a gameobject in certain distance then set the gameobject back to original 
position relative to the gameobject that moved.

## Variables

| Name              | Description                                                                |
|:------------------|:---------------------------------------------------------------------------|
| mResetTrans       | Reset to this position. If this is null, we use original position instead. |
| mDistance         | How long this gameobject could travel.                                     |
| mUseLocalPosition | Use the local position instead of global position.                         |
