# JCS_2DSlideScreenCamera

Camera for GUI or canvas space.

## Variables

| Name              | Description                                 |
|:------------------|:--------------------------------------------|
| afterSwiped       | Function call after the scene changed       |
| afterSwiped       | Function call after the user has swiped     |
| UnityGUIType      | GUI type.                                   |
| PanelHolder       | Slide screen panel holder for this camera.  |
| InteractableSwipe | If true, allow the mobile swipe action.     |
| SwipeArea         | Area space to swipe for previous/next page. |
| FreezeX           | Freeze the x axis sliding action.           |
| FreezeY           | Freeze the y axis sliding action.           |
| SwitchSceneSound  | Sound when trigger switch scene.            |
| CurrentPage       | Page start from center.                     |
| MinPageX          | Minimum page on x-axis.                     |
| MaxPageX          | Maximum page on x-axis.                     |
| MinPageY          | Minimum page on y-axis.                     |
| MaxPageY          | Maximum page on y-axis.                     |

## Functions

| Name        | Description                                     |
|:------------|:------------------------------------------------|
| SetPage     | Set the page using page index. (Vector 2)       |
| SetPageX    | Set the page using page index. (x-axis)         |
| SetPageY    | Set the page using page index. (y-axis)         |
| SwitchScene | Swicth the scene by sliding its with direction. |
