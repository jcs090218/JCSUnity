# JCS_ButtonSelection

Button selection must work with button selection group, which is
[JCS_ButtonSelectionGroup](?page=Input_sl_JCS_ButtonSelectionGroup).

## Variables

| Name             | Description                                                        |
|:-----------------|:-------------------------------------------------------------------|
| mDeactiveAtAwake | Deactive this selection on Awake time.                             |
| mSkip            | Skip this selection, so this selection cannot be selected.         |
| mSelectedEvent   | Events when you enter this selection.                              |
| mEffects         | List of effect when this selection on/off.                         |
| mButton          | The button for this selection.                                     |
| mSelfAsButton    | This game object itself is a button and use this button component. |
| mUpSelection     | "What is the selection ontop of this selection? (Press Up)         |
| mDownSelection   | What is the selection ontop of this selection? (Press Down)        |
| mRightSelection  | What is the selection ontop of this selection? (Press Right)       |
| mLeftSelection   | What is the selection ontop of this selection? (Press Left)        |

## Functions

| Name        | Description                                |
|:------------|:-------------------------------------------|
| DoSelection | Do stuff when this selection been checked. |
| IsSelected  | Is the selection is selected currently.    |
| MakeSelect  | Make this selection selected.              |
