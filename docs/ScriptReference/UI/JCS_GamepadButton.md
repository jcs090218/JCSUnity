# JCS_GamepadButton

Button will listen to the gamepad.

## Variables

| Name                       | Description                                                              |
|:---------------------------|:-------------------------------------------------------------------------|
| mListenToAnyKey            | Ignore the top two variables, listen to any key on the keyboard/gamepad. |
| mKeyActionType             | Key action type.                                                         |
| mKKeyToListen              | Key to trigger this button.                                              |
| mJKeyToListen              | Key to trigger this button.                                              |
| mJoystickLitener           | Which joystick should listen?                                            |
| mPlayWithGlobalSoundPlayer | Play with the global sound player.                                       |
| mButtonClickSound          | Sound when button is pressed.                                            |
| mSoundMethod               | Sound method.                                                            |

## Example

ExampleGamepadButton.cs

```cs
  public class ExampleGamepadButton : JCS_GamepadButton {
      public override JCS_OnClickCallback() {
          Debug.Log("Gamepad button click!");
      }
  }
```
