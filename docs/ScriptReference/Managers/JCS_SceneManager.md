# JCS_SceneManager

Better version of scene manager.

## Variables

| Name                        | Description                                   |
|:----------------------------|:----------------------------------------------|
| mSwitchSceneType            | Type/Method to switch the scene.              |
| mPauseGameWhileLoadingScene | Pause the game while the scene start to load. |
| mAlign                      | Which direction to fade slide.                |
| mOverrideSetting            | Do this scene using the specific setting.     |
| mSceneFadeInTime            | Fade in time. (For this scene)                |
| mSceneFadeOutTime           | Fade out time. (For this scene)               |

## Functions

| Name             | Description                                        |
|:-----------------|:---------------------------------------------------|
| LoadScene        | Load the scene with the default switch scene type. |
| IsSwitchingScene | Is currently switching the scene?                  |
