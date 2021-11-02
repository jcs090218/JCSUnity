# JCS_ScreenSettings

Screen related settings.

## Variables

| Name                                    | Description                                                                      |
|:----------------------------------------|:---------------------------------------------------------------------------------|
| onScreenResize                          | Callback when the screen resized.                                                |
| onScreenIdle                            | Callback when the screen is not resizing.                                        |
| RESIZE_TO_ASPECT_WHEN_APP_STARTS        | Resize the screen/window to certain aspect when the application starts.          |
| RESIZE_TO_STANDARD_WHEN_APP_STARTS      | Resize the screen/window to standard resoltuion when application starts.         |
| RESIZE_TO_ASPECT_EVERYTIME_SCENE_LOADED | Resize the screen/window everytime a scene are loaded.                           |
| RESIZE_TO_SMALLER_EDGE                  | When resize, resize to the smaller edge, if not true will resize to larger edge. |
| mResizablePanelsColor                   | Defualt color to aspect panels.                                                  |
| STANDARD_SCREEN_SIZE                    | Standard screen size to calculate the worldspace obejct's camera view.           |
| SCREEN_TYPE                             | Type of the screen handle.                                                       |

## Functions

| Name                       | Description                                                                      |
|:---------------------------|:---------------------------------------------------------------------------------|
| ShouldSpawnResizablePanels | Return true, if we should use resizalbe panels.                                  |
| IsResponsive               | Return true, if current screen type is responsive.                               |
| StartingScreenSize         | Return the starting screen size by the current screen type.                      |
| ScreenRatio                | Return the ratio from expected screen size to actual screen size.                |
| BlackspaceWidth            | Return width of the blackspace on the screen, if any after resizing the screen.  |
| BlackspaceHeight           | Return height of the blackspace on the screen, if any after resizing the screen. |
| VisibleScreenWidth         | Get the visible of the screen width.                                             |
| VisibleScreenHeight        | Get the size of the screen height.                                               |
| ForceAspectScreenOnce      | Make the screen in certain aspect ratio.                                         |
| ForceStandardScreenOnce    | Resize the screen resolution to standard resolution once.                        |
