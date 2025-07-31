# JCS_Instance

Singleton instance interface.

## Functions

| Name               | Description                |
|:-------------------|:---------------------------|
| RegisterInstance   | Register a scene instance. |
| DeregisterInstance | Deregister the instance.   |
| FirstInstance      | Return the first instance. |
| GetInstance        | Return the instance.       |

### 🏛️ Example

See `Example.cs`:

```cs
  public class Example : JCS_Instance<Example>
  {
      private void Awake ()
      {
          RegisterInstance(this);
      }
	  
      protected override void TransferData(Example _old, Example _new)
      {
          // ..
      }
  }
```
