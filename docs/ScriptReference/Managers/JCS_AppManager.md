# JCS_AppManager

Interface communicate to application layer.

## Variables

| Name                    | Description                                    |
|:------------------------|:-----------------------------------------------|
| onSystemLanguageChanged | Execute after the system language has changed. |
| RequestCamera           | Request permission for camera/webcam.          |
| RequestMicrophone       | Request permission for microphone.             |
| RequestLocation         | Request permission for location service.       |
| SimulatePlatformType    | Target platform type to simulate.              |
| PlatformType            | This will override Platform Type.              |
| systemLanguage          | Current systme language.                       |
| SimulateSystemLanguage  | If true, override current system language.     |
| SimulateLanguage        | Target language to simulate.                   |

## Functions

| Name                    | Description                                                  |
|:------------------------|:-------------------------------------------------------------|
| IsPC                    | Return true if current platform is personal computer.        |
| IsMobile                | Return true if current platform is mobile phone.             |
| Quit                    | Quit the application.                                        |
| AddLangText             | Register a new language text.                                |
| RefreshLangTexts        | Refresh all languages text in game.                          |
| StartRequestCamera      | Start to request permission for camera/webcam.               |
| StartRequestMicrophone  | Start to request permission for microphone.                  |
| StartRequestLocation    | Start to request permission for location service.            |
| StartRequestPermissions | Iterate through all the services, and enable it by settings. |
