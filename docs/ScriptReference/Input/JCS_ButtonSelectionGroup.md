# JCS_ButtonSelectionGroup

Group multiple selection and handle it so you can tell which selection is
selected.

## Variables

| Name                     | Description                                                                            |
|:-------------------------|:---------------------------------------------------------------------------------------|
| mOnEnableResetSelections | Reset the selection when the selection is enabled.                                     |
| mStartingSelection       | While reseting the selections this will get choose to be the first selected selection. |
| mSelections              | List of all the selections this group handle."                                         |

## Functions

| Name                          | Description                                                |
|:------------------------------|:-----------------------------------------------------------|
| AddSelection                  | Add a selection into list.                                 |
| RemoveSelection               | Remove a selection from the list.                          |
| ResetAllSelections            | Reset the selection group to starting status.              |
| CloseAllSelections            | Close the selection area. Nothing will be high-lighted.    |
| OkaySelection                 | Run the selection button.                                  |
| NextSelection                 | Change to the next button selection. (One dimensional)     |
| PrevSelection                 | Change to the previous button selection. (One dimensional) |
| SelectSelection               | Selection this selection.                                  |
| UpSelection                   | Select the selection on the top. (Two Dimensional)         |
| DownSelection                 | Select the selection on the down. (Two Dimensional)        |
| RightSelection                | Select the selection on the right. (Two Dimensional)       |
| LeftSelection                 | Select the selection on the left. (Two Dimensional)        |
| GetButtonSelectionByDirection | Get the selection depends on the direction.                |
