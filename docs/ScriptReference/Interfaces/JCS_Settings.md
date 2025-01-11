# JCS_Settings

Interface of all setting class.

## Variables

| Name     | Description                                                 |
|:---------|:------------------------------------------------------------|
| instance | Hold the settings class instance for ensuring is singleton. |

## Functions

| Name           | Description                                        |
|:---------------|:---------------------------------------------------|
| CheckInstance  | Force the setting object singleton.                |
| TransferData   | Transfter data through one scene to another scene. |

## Example

Example.cs

```cs
  public class Example : JCS_Settings<Example>
  {
      private void Awake ()
      {
          // Make the instance singleton.
          instance = CheckInstance(instance, this);
      }

      protected override void TransferData(Example _old, Example _new)
      {
          // ..
      }
  }
```
