# JCS_DialogueObject

Base class of Game Window.

## Variables

| Name            | Description                                         |
|:----------------|:----------------------------------------------------|
| mDialogueIndex  | Unique index to open the dialogue.                  |
| mKeyCode        | Key to open this dialogue.                          |
| DialogueType    | Dialogue type for priority.                         |
| PanelType       | Type for positioning the panel every time it opens. |
| OpenWindowClip  | Sound when open this dialouge window.               |
| CloseWindowClip | Sound when close this dialouge window.              |

## Functions

| Name                     | Description                                                                                                  |
|:-------------------------|:-------------------------------------------------------------------------------------------------------------|
| DoPanelType              | Decide what panel is this panel going to be.                                                                 |
| DestroyDialogue          | Destroy this dialgoue.                                                                                       |
| ShowDialogueWithoutSound | Show the dialogue in the game without the sound.                                                             |
| HideDialogueWithoutSound | Hide the dialogue without the sound.                                                                         |
| MoveToTheLastChild       | Move the last child of the current child will make the panel in front of any other GUI in the current panel. |
| ToggleVisibility         | Toggle this dialgoue show and hide.                                                                          |
