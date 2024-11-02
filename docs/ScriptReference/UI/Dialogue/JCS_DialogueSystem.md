# JCS_DialogueSystem

Dialogue system core implementation.

## Variables

| Name                              | Description                                        |
|:----------------------------------|:---------------------------------------------------|
| onDispose                         | Callback when successfully dispose the dialogue.   |
| mMakeHoverSelect                  | If the mouse hover then select the selection.      |
| mCenterImage                      | Image displayed at the center.                     |
| mLeftImage                        | Image displayed at the left.                       |
| mRightImage                       | Image displayed at the right.                      |
| mNameTag                          | Text box to display names.                         |
| mTextBox                          | Text box to display content messages.              |
| mScrollTime                       | Speed of scrolling the text.                       |
| mProgressOnHidden                 | Allow progress dialogue even the it is hidden.     |
| mButtonSelectionGroup             | Make control from the gamepad.                     |
| mCompleteTextBeforeAction         | Complete text before run action.                   |
| mCompleteTextBeforeActionOnButton | Complete text before run action on button's event. |
| mAutoProgress                     | If true, auto progress the dialgoue.               |
| mAutoDelay                        | Delay before start the next text popup.            |
| mActiveSound                      | Sound plays when active dialogue.                  |
| mDisposeSound                     | Sound plays when dispose dialogue.                 |

## Functions

| Name              | Description                                                     |
|:------------------|:----------------------------------------------------------------|
| ActiveDialogue    | Start the dialogue, in other word same as start a conversation. |
| RunAction         | Run the script once.                                            |
| SendChoice        | Send a choice to current status.                                |
| SendNext          | Next button is the only option for current status.              |
| SendNextPrev      | Current status will be next and prev control/options.           |
| SendOk            | Okay button for only option.                                    |
| SendYesNo         | Yes/No options for current status.                              |
| SendSimple        | Only exit button will be the only option.                       |
| SendAcceptDecline | Accept/Decline options.                                         |
| SendEmpty         | Send Empty option, expect selections take over it.              |
| Dispose           | Call this to end the dialogue status.                           |
| ResetStats        | Reset all dialogue system.                                      |
| WorldMessage      | Send a world message, online mode only.                         |
| SendNameTag       | Set the current status name tag.                                |
| SendCenterImage   | Set the sprite to the image component. (Center)                 |
| SendLeftImage     | Set the sprite to the image component. (Left)                   |
| SendRightImage    | Set the sprite to the image component. (Right)                  |
| IncPage           | Increament one from page.                                       |
| DecPage           | Decreament one from page.                                       |
| Previous          | Go to previous page of the dialogue.                            |
| Next              | Go to next page of the dialogue.                                |
| NextOrDispose     | Like `Next` function but dispose it when possible.              |
| IsScrolling       | Return true if the dialogue system is still animating the text. |
| SkipToEnd         | Skip the current text scroll.                                   |
| IsVisible         | Return true if the dialgoue system is visible.                  |
| Show              | Show the dialogue.                                              |
| Hide              | Hide the dialogue.                                              |
| ToggleVisiblity   | Toggle between show and hide.                                   |
