# JCS_Webcam

Basic webcam class usage.

## Variables

| Name             | Description                                                   |
|:-----------------|:--------------------------------------------------------------|
| ManuallySetSize  | Manually preserve the size in scene.                          |
| MustBeFullScreen | Make webcam maximize to the widest edge.                      |
| FPS              | FPS for webcam.                                               |
| ResumeTime       | After the screenshot is taken, how fast to resume the webcam. |
| mDeltaTimeType   | Type of the delta time.                                       |
| DelayTime        | Delay time to fade out the splash screen.                     |
| TakePicKey       | Key to take webcam image.                                     |
| TakePhotoSound   | Sound when taking the screenshot.                             |
| isPlaying        | Flag to check if current webcam active.                       |

## Functions

| Name               | Description                                                |
|:-------------------|:-----------------------------------------------------------|
| ActiveWebcam       | Start the webcam if it is detected on your machine.        |
| TakeSnapShotWebcam | Take a snap shot and save it in the application data path. |
| SavePath           | Get the webcam images' save path.                          |
| LastImageFileIndex | Last webcam image's file index.                            |
| ImagePathByIndex   | Form webcam image path by index.                           |
| LoadImageByIndex   | Load webcam image by file index.                           |
| LoadAllImages      | Load all webcam images.                                    |
| DeleteImageByIndex | Delete webcam image by image file's index.                 |
| DeleteAllImages    | Delete all webcam images from disk.                        |
| Play               | Start the webcam.                                          |
| Pause              | Pause the webcam.                                          |
| Stop               | Stop the webcam.                                           |
