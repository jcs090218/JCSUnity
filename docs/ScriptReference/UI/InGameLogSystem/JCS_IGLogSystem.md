# JCS_IGLogSystem

In game log system.

## Variables

| Name        | Description                     |
|:------------|:--------------------------------|
| mLogSpacing | Space between each log message. |

## Functions

| Name                | Description                                      |
|:--------------------|:-------------------------------------------------|
| SendLogMessage      | Make single log message on the screen.           |
| SendLogMessages     | Make multiple log messages on the screen.        |
| RemoveFromRenderVec | Remove the log message that are outdated.        |
| UpdateSpace         | Update all current active log messages' spacing. |

## Support Encodings

If you try to display a message but it displays `?` question mark instead.

```cs
var igl = JCS_UtilitiesManager.instance.GetIGLogSystem();
igl.SendLogMessage("Unicode text here!");
```

Make sure you have save your C# script in the correct file encoding.
