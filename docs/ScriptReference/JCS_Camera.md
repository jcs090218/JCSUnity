# JCS_Camera

Camera based class.

## Variables

| Name                    | Description                                                        |
|:------------------------|:-------------------------------------------------------------------|
| mDisplayGameDepthCamera | Display the camera depth.                                          |
| mGameCamColor           | The color of the camera depth.                                     |
| mFollowing              | Flag to check if currently the camera following the target object. |
| PositionOffset          | Offset the camera position from its' original position.            |
| mTimeType               | Type of the delta time.                                            |
| SmoothTrack             | Flag to check if using smooth track, otherwise hard track.         |
| mBoundary               | The movement boundary.                                             |

## Functions

| Name               | Description                                          |
|:-------------------|:-----------------------------------------------------|
| TakeScreenshot     | Take a snap shot in the game. (Standalone only)      |
| SavePath           | Get the screenshot images' save path.                |
| LastImageFileIndex | Last screenshot image's file index.                  |
| ImagePathByIndex   | Form screenshot image path by index.                 |
| LoadImageByIndex   | Load screenshot image by file index.                 |
| LoadAllImages      | Load all screenshot images.                          |
| DeleteImageByIndex | Delete screenshot image by image file's index.       |
| DeleteAllImages    | Delete all screenshot images from disk.              |
| CheckInScreenSpace | Check if a game object is shown in the screen space. |
| WorldToCanvasSpace | Convert world space point to canvas space point.     |
| CanvasToWorldSpace | Convert canvas space point to world space point.     |
| SetPosition        | Proper way to set camera position.                   |
