# JCS_Instance

Singleton instance interface.

This file contain three classes.

### 🏛️ JCS_Instance

See `Example.cs`:

```cs
  public class Example : JCS_Instance<Example>
  {
      private void Awake ()
      {
          instance = this;
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
  }
```
