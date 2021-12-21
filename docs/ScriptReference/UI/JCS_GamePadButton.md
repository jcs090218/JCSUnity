# JCS_GamePadButton

Button will listen to the gamepad.

## Variables

| Name                       | Description                                                              |
|:---------------------------|:-------------------------------------------------------------------------|
| mIgnorePauseCheck          | Still check input when the game is pause?                                |
| mListenToAnyKey            | Ignore the top two variables, listen to any key on the keyboard/gamepad. |
| mKeyActionType             | Key action type.                                                         |
| mKKeyToListen              | Key to trigger this button.                                              |
| mJKeyToListen              | Key to trigger this button.                                              |
| mJoystickLitener           | Which joystick should listen?                                            |
| mPlayWithGlobalSoundPlayer | Play with the global sound player.                                       |
| mButtonClickSound          | Sound when button is pressed.                                            |
| mSoundMethod               | Sound method.                                                            |

## Example

ExampleGamePadButton.cs

```cs
  public class ExampleGamePadButton : JCS_GamePadButton {
      public override JCS_OnClickCallback() {
          Debug.Log("Gamepad button click!");
      }
  }
```
