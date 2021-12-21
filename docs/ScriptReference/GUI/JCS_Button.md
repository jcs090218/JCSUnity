# JCS_Button

Button wrapper for more usable functionalities.

## Variables

| Name              | Description                                               |
|:------------------|:----------------------------------------------------------|
| mButtonText       | Text component under the button.                          |
| mButtonSelection  | Button Selection for if the button that are in the group. |
| mAutoListener     | Auto add listner to button click event?"                  |
| mDialogueIndex    | Index/ID for record the dialogue instance.                |
| mInteractable     | Is the button interactable or not.                        |
| mInteractColor    | Color tint when button is interactable.                   |
| mNotInteractColor | Color tint when button is not interactable.               |

## Example

ExampleButton.cs

```cs
  class ExampleButton : JCS_Button {
      public override void OnClick() {
          Debug.Log("On click!"");
      }
  }
```
