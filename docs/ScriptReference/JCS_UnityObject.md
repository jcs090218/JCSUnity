# JCS_UnityObject

Interface handle between multiple type of Unity Engine's declare type of game object.
This class makes sure each game object will still use the same variable name.

For instance, if you want to enable/disable an game object. By using this interface
you do not need to know what exactly the component you are trying to enable
/disable. You can just call `GetComponent<JCS_UnityObject>().LocalEnable = true;`
instead of trying to identify the component type and change the specific variable
from its component.

## Variables

| Name             | Description                                                 |
|:-----------------|:------------------------------------------------------------|
| mMakeUnique      | Make material clone.                                        |
| mColorProps      | Shader's color properties.                                  |
| LocalType        | Component of this current type.                             |
| LocalTransform   | Either regular transform or recttransform base on the type. |
| LocalMaterial    | Local material of this game object.                         |
| Position         | Position of this game object.                               |
| LocalPosition    | Local position of this game object.                         |
| EulerAngles      | Euler angles of this game object.                           |
| LocalEulerAngles | Local euler angles of this game object.                     |
| LocalScale       | Local scale of this game object.                            |
| LocalEnabled     | Enabled/disable this game object.                           |
| LocalColor       | Color of this game object.                                  |
| LocalAlpha       | Alpha channel of this game object.                          |
| LocalRed         | Red channel of this game object.                            |
| LocalGreen       | Green channel of this game object.                          |
| LocalBlue        | Blue channel of this game object.                           |
| LocalMainTexture | Main texture of this game object.                           |
| LocalSprite      | Sprite of this game object.                                 |
| LocalIsVisible   | Is the game object visible? Or set visible.                 |
| LocalFlipX       | Flip the game object in x-axis.                             |
| LocalFlipY       | Flip the game object in y-axis.                             |

## Functions

| Name            | Description                                                                               |
|:----------------|:------------------------------------------------------------------------------------------|
| UpdateUnityData | Identify the object itself. See [JCS_UnityObjectType](?page=Enums_sl_JCS_UnityObjectType) |
| UpdateMaterial  | Make unique material.                                                                     |
| GetObjectType   | Returns the type of this unity object.                                                    |
| IsObjectType    | Check if TYPE is current object type.                                                     |
