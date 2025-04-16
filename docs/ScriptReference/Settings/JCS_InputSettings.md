# JCS_InputSettings

Handle cross platform input settings.

## Variables

| Name                | Description                                  |
|:--------------------|:---------------------------------------------|
| joystickNames       | List of the input device name.               |
| mTargetGamePad      | Targeting game pad going to use in the game. |
| mTotalGamePadInGame | Total maxinum game pad will live in game.    |

## Functions

| Name                    | Description                                              |
|:------------------------|:---------------------------------------------------------|
| GetJoystickButtonName   | Get the joystick button name by joystick button label.   |
| GetJoystickButtonIdName | Get the joystick button name by joystick button label.   |
| GetJoystickButtonIdName | Get the joystick button name by joystick button label.   |
| IsAxisJoystickButton    | Is the button label a axis joystick value?               |
| GetPositiveNameByLabel  | Get Unity's controller naming convention.                |
| IsInvert                | Check if any specific button's buffer need to be invert. |
| GetAxisChannel          | Get axis channel for Unity's built-in InputManager.      |
| GetAxisType             | Get the axis type depends on the joystick button label.  |
