# JCS_InstanceOld

Singleton instance that keep the old one and delete the new one.

## Functions

| Name           | Description                                        |
|:---------------|:---------------------------------------------------|
| CheckInstance  | Force the setting object singleton.                |
| TransferData   | Transfter data through one scene to another scene. |

### 🏛️ Example

See `Example.cs`:

```cs
  public class ExampleNew : JCS_InstanceNew<Example>
  {
      private void Awake ()
      {
          CheckInstance(this);
      }

      protected override void TransferData(ExampleNew _old, ExampleNew _new)
      {
          // ..
      }
  }
```
