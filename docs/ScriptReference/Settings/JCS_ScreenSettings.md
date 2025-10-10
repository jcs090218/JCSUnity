# JCS_ScreenSettings

Screen related settings.

## Variables

| Name                               | Description                                                                      |
|:-----------------------------------|:---------------------------------------------------------------------------------|
| onChanged                          | Callback when screen changed.                                                    |
| onChangedResolution                | Callback when screen resolution changed.                                         |
| onChangedMode                      | Callback when screen mode changed.                                               |
| onResizableResize                  | Callback when the screen resized in resizable mode.                              |
| onResizableIdle                    | Callback when the screen is not resizing in resizable mode.                      |
| resizeToAspectWhenAppStarts        | Resize the screen/window to certain aspect when the application starts.          |
| resizeToStandardWhenAppStarts      | Resize the screen/window to standard resoltuion when application starts.         |
| resizeToAspectEverytimeSceneLoaded | Resize the screen/window everytime a scene are loaded.                           |
| resizeToSmallerEdge                | When resize, resize to the smaller edge, if not true will resize to larger edge. |
| mResizablePanelsColor              | Defualt color to aspect panels.                                                  |
| standardSize                       | Standard screen size to calculate the worldspace obejct's camera view.           |
| screenType                         | Type of the screen handle.                                                       |

## Functions

| Name                       | Description                                                                      |
|:---------------------------|:---------------------------------------------------------------------------------|
| ShouldSpawnResizablePanels | Return true, if we should use resizalbe panels.                                  |
| IsResponsive               | Return true, if current screen type is responsive.                               |
| StartingSize         | Return the starting screen size by the current screen type.                      |
| ScreenRatio                | Return the ratio from expected screen size to actual screen size.                |
| BlackspaceWidth            | Return width of the blackspace on the screen, if any after resizing the screen.  |
| BlackspaceHeight           | Return height of the blackspace on the screen, if any after resizing the screen. |
| VisibleScreenWidth         | Get the visible of the screen width.                                             |
| VisibleScreenHeight        | Get the size of the screen height.                                               |
| ForceAspectScreenOnce      | Make the screen in certain aspect ratio.                                         |
| ForceStandardScreenOnce    | Resize the screen resolution to standard resolution once.                        |
