# JCS_UnityObject

Interface handle between multiple type of Unity Engine's declare type of gameobject.
This class makes sure each gameobject will still use the same variable name.

For instance, if you want to enable/disable an gameobject. By using this interface
you do not need to know what exactly the component you are trying to enable
/disable. You can just call `GetComponent<JCS_UnityObject>().LocalEnable = true;`
instead of trying to identify the component type and change the specific variable
from its component.

## Variables

| Name             | Description                                                 |
|:-----------------|:------------------------------------------------------------|
| LocalType        | Component of this current type.                             |
| LocalTransform   | Either regular transform or recttransform base on the type. |
| Position         | Position of this gameobject.                                |
| LocalPosition    | Local position of this gameobject.                          |
| EulerAngles      | Euler angles of this gameobject.                            |
| LocalEulerAngles | Local euler angles of this gameobject.                      |
| LocalScale       | Local scale of this gameobject.                             |
| LocalEnabled     | Enabled/disable this gameobject.                            |
| LocalColor       | Color of this gameobject.                                   |
| LocalAlpha       | Alpha channel of this gameobject.                           |
| LocalRed         | Red channel of this gameobject.                             |
| LocalGreen       | Green channel of this gameobject.                           |
| LocalBlue        | Blue channel of this gameobject.                            |
| LocalMainTexture | Main texture of this gameobject.                            |
| LocalSprite      | Sprite of this gameobject.                                  |
| LocalIsVisible   | Is the gameobject visible? Or set visible.                  |
| LocalFlipX       | Flip the gameobject in x-axis.                              |
| LocalFlipY       | Flip the gameobject in y-axis.                              |

## Functions

| Name            | Description                                                                               |
|:----------------|:------------------------------------------------------------------------------------------|
| UpdateUnityData | Identify the object itself. See [JCS_UnityObjectType](?page=Enums_sl_JCS_UnityObjectType) |
| GetObjectType   | Returns the type of this unity object.                                                    |
| IsObjectType    | Check if TYPE is current object type.                                                     |
