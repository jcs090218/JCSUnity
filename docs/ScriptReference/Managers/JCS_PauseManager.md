# JCS_PauseManager

If you are some kind of game that needs the pause screen, you definitely
need the pause manager to add it to `JCS_Managers` transform in the Hierarchy.

## Variables

| Name                       | Description                                       |
|:---------------------------|:--------------------------------------------------|
| mDefaultTimeScale          | The default time scale.                           |
| mResizePauseActionListTime | Time to resize the pause action list, in seconds. |
| mAsymptotic                | Pause and unpause with asymptotic transition.     |
| mFriction                  | How fast the asymptotic transition?               |

## Functions

| Name              | Description                                                                                            |
|:------------------|:-------------------------------------------------------------------------------------------------------|
| AddActionToList   | Add the pause action to the list of pause action list, in order to get manage by this "pause manager". |
| ResetState        | Reset pause state.                                                                                     |
| Pause             | Pause the game.                                                                                        |
| Unpase            | Unpause the game.                                                                                      |
| PauseTheWholeGame | ause/Unpause the whole game.                                                                           |
