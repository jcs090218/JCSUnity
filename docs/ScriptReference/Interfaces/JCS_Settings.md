# JCS_Settings

Interface of all setting class.

## Variables

| Name     | Description                                                 |
|:---------|:------------------------------------------------------------|
| instance | Hold the settings class instance for ensuring is singleton. |

## Functions

| Name           | Description                                        |
|:---------------|:---------------------------------------------------|
| CheckSingleton | Force the setting object singleton.                |
| TransferData   | Transfter data through one scene to another scene. |

## Example

ExampleSetting.cs

```cs
  public class ExampleSetting : MonoBehaviour
  {
      private void Awake ()
      {
          // Make the instance singleton.
          instance = CheckSingleton(instance, this);
      }
  }
```
