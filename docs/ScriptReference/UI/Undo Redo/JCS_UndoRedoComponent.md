# JCS_UndoRedoComponent

Undo Redo system component.

## Variables

| Name            | Description                                                                         |
|:----------------|:------------------------------------------------------------------------------------|
| mUndoRedoSystem | Undo redo system, if not filled will be use the universal undo redo system instead. |
| mFocusAfterUndo | Focus component after undo.                                                         |
| mFocusAfterRedo | Focus component after redo.                                                         |

## Functions

| Name                    | Description                                      |
|:------------------------|:-------------------------------------------------|
| Undo                    | Undo this component.                             |
| Redo                    | Redo this compnent.                              |
| StopRecording           | Stop recording undo/redo.                        |
| StartRecording          | Start recording undo/redo.                       |
| IsRecording             | Is current component recording undo/redo action? |
| ClearAllUndo            | Clear all undo history.                          |
| ClearAllRedo            | Clear all the redo history.                      |
| ClearAllUndoRedoHistory | Clear all undo and redo history.                 |
