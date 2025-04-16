# JCS_Instance

Singleton instance interface.

This file contain three classes.

## Variables

| Name     | Description                                                 |
|:---------|:------------------------------------------------------------|
| instance | Hold the settings class instance for ensuring is singleton. |

## Functions

| Name           | Description                                        |
|:---------------|:---------------------------------------------------|
| CheckInstance  | Force the setting object singleton.                |
| TransferData   | Transfter data through one scene to another scene. |

### 🏛️ JCS_Instance

See `Example.cs`:

```cs
  public class Example : JCS_Instance<Example>
  {
      private void Awake ()
      {
          instance = this;
      }
	  
      protected override void TransferData(Example _old, Example _new)
      {
          // ..
      }
  }
```

### 🏛️ JCS_InstanceOld

Singleton instance interface to keep the old instance.

See `ExampleOld.cs`:

```cs
  public class ExampleOld : JCS_InstanceOld<Example>
  {
      private void Awake ()
      {
          CheckInstance(this);
      }

      protected override void TransferData(ExampleOld _old, ExampleOld _new)
      {
          // ..
      }
  }
```

### 🏛️ JCS_InstanceNew

Check singleton for keep the old one.

See `ExampleNew.cs`:

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
