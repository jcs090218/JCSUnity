# JCS_Canvas

Control of the canvas component.

## Variables

| Name            | Description                                      |
|:----------------|:-------------------------------------------------|
| mDisplayOnAwake | If true, show on awake time; otherwise, hide it. |
| mMainCanvas     | Resizable screen will be attach to this canvas.  |
| mActiveSound    | Play sound when active the canvas.               |
| mDeactiveSound  | Play sound when deactive the canvas.             |

## Functions

| Name                       | Description                                                   |
|:---------------------------|:--------------------------------------------------------------|
| GuessCanvas                | Return the `canvas` that is the parent of the `trans` object. |
| AddComponentToResizeCanvas | Add component to resize canvas.                               |
| Show                       | Show the canvas, so it's visible on the screen.                              |
| Hide                       | Hide the canvas, so it's invisible on the screen.                            |
