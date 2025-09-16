# JCS_SceneManager

Better version of scene manager.

## Variables

| Name               | Description                                             |
|:-------------------|:--------------------------------------------------------|
| onSwitchSceneInit  | Custom callback when initialize stage.                  |
| onSwitchSceneLoad  | Custom callback when on load scene stage.               |
| onSwitchSceneIn    | Custom callback when completed the fade in transition.  |
| onSwitchSceneOut   | Custom callback when completed the fade out transition. |
| mSwitchSceneType   | Type/Method to switch the scene.                        |
| mOverlaySceneNames | A list of addictive scene to load.                      |
| mOverlayUseAsync   | Load additive overlay scenes asynchronously.            |
| mAlign             | Which direction to fade slide.                          |
| mOverrideSetting   | Do this scene using the specific setting.               |
| mSceneFadeInTime   | Fade in time. (For this scene)                          |
| mSceneFadeOutTime  | Fade out time. (For this scene)                         |

## Functions

| Name                       | Description                                                        |
|:---------------------------|:-------------------------------------------------------------------|
| RegisterOverlaySceneLoaded | Register an event call after the targeted overlay scene is loaded. |
| LoadScene                  | Load the target scene.                                             |
| ReloadScene                | Reload the current scene.                                          |
| LoadNextScene              | Load the next scene.                                               |
| NextSceneName              | Return the next scene name.                                        |
| GetSceneNameByOption       | Return the scene name by their options.                            |
| IsSwitchingScene           | Check is loading the scene or not.                                 |
| GetAllScenes               | Return a list of all scenes.                                       |
