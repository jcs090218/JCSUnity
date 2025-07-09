# JCS_CanvasComp

Component that requires the component `JCS_Canvas` without actually inherit the class `JCS_Canvas`.

## Variables

| Name       | Description                                |
|:-----------|:-------------------------------------------|
| mCanvas    | The canvas this component to control.      |
| onShow     | Execution when canvas is shown.            |
| onHide     | Execution when canvas is hidden.           |
| onShowFade | Execution when canvas is shown by fading.  |
| onHideFade | Execution when canvas is hidden by fading. |

## Functions

| Name             | Description                                       |
|:-----------------|:--------------------------------------------------|
| IsShown          | Return true if the canvas is currently visible.   |
| Show             | Show the canvas, so it's visible on the screen.   |
| Hide             | Hide the canvas, so it's invisible on the screen. |
| ToggleVisibility | Toggle the canvas' visibility.                    |
| IsFading         | Return true if the canvas is fading in/out.       |
