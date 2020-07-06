# Change Log

All notable changes to this project will be documented in this file.

Check [Keep a Changelog](http://keepachangelog.com/) for recommendations on how to structure this file.


### 2020-07-06

* Fixed load all images last index logic issue from `JCS_Webcam`.
* Fixed load all images last index logic issue from `JCS_Camera`.
* Fixed image file not exist load for `JCS_ImageLoader`.

### 2020-07-05

* Added more Camera screenshot image's API.
* Added more Webcam image's API.

### 2020-07-03

* Updated webcam image utility functions.
* Implemented new component `JCS_ScreenshotButton`.
* Added remove from dir utility function.
* Added util function for webcam images.
* Added util function for screenshot images.
* Organized and added webcam settings to game settings.

### 2020-07-02

* Updated webcam stopping logic.
* Fixed webcam texture remaining to the next scene issue.

### 2020-07-01

* Fixed resize panel logic from `JCS_PanelRoot` component.

### 2020-06-30

* Updated `JCS_Webcam` for webcam module.
* Supplied `JCS_UnityObject` for generic Unity render object.
* Supplied `JCS_ColliderObject` for generic collider object.

### 2020-06-26

* Organized source code for better document string.
* Removed unused variables in `JCS_3DCamera` component.

### 2020-06-23

* Renamed framework scenes to a better standard.

### 2020-06-20

* Implemented `JCS_OrderEvent` component.

### 2020-06-14

* Updated to have tween panel shown ontop when active.

### 2020-06-10

* Fixed minor logic error from testing component.

### 2020-06-07

* Updated the create section from edtiro module for better UX.

### 2020-06-05

* Differentiate the base panel and dialgoue panel for JCSUnity editor.

### 2020-06-01

* Implemented switch BGM button component for GUI module.

### 2020-05-12

* Organized sound player attachment default behaviour.
* Removed `JCS_2DShakeEffect`, please use `JCS_3DShakeEffect` instead.

### 2020-05-11

* Upgraded 3D walk action from 3D AI module.

### 2020-05-08

* Added panels section for GUI utility functions.
* Removed `GUI/Button/Dialogue` for repetitive functionalities with  other components/scripts.
* Implemented `JCS_ColliderType` enum for collider identification.
* Implemented `JCS_ColliderObject` for collider component management for single transform.

### 2020-05-07

* Added 3D walk action manager.
* Removed indie standard, use `others`/`2D`/`3D` instead.
* Fixed 3D walk action overlap destination search path logic error.

### 2020-05-05

* Released more mix damage text pool API.
* Fixed `Pythagorean Theorem` calculation in math module.
* Fixed 3D Walk action AI with remaining distance calculation.

### 2020-05-03

* Organized shader sources.
* Updated export settings so it no longer export frame test files.

### 2020-05-01

* Updated the destory with time module with better flexible control.

### 2020-04-30

* Upgraded the liquid bar module for better understanding API calls.
* Upgraded the damage text system for better default UX.

### 2020-04-28

* Updated walk action with remaining distance accept variable.

### 2020-04-27

* Now damage text system supports 3D space.
* Upgrade damage text system's user experience.

### 2020-04-26

* Updated 3D walk action for AI module.

### 2020-04-22

* Updated with detect touch count for touch event.

### 2020-04-21

* Revert tween panel active/deactive logic.
* Added over GUI element check for utility module.
* Make `slide input` and `mobile mouse event` as optional to 
 prevent multiple same scripts' execution.

### 2020-04-20

* Add check scene utility function.

### 2020-04-18

* Implemented `JCS_ValueTweener` compnent for tweener module.
* Implemented `JCS_3DLight` compnent for 3D module.

### 2020-04-15

* Updated the 3D AI module in walk action.

### 2020-04-14

* Added range include for float value.

### 2020-04-11

* Added the path action for path finding module.
* Upgraded 3D camera revolution rotate action, is more stable and 
 has the improvement of the UX.

### 2020-04-07

* Implemented `JCS_3DCameraPlayer` for character control relatives 
 to camera, reference game `Monster Hunter`.

### 2020-04-06

* Implemented `JCS_3DHintBubble` component to `GameObject` module.

### 2020-04-03

* Implemented `JCS_SimplePathAction` component.
* Now tween panel support multiple transform tweeners.

### 2020-04-02

* Implemented toggle panel default button.
* Added dispose callback for dialogue system.
* Updated the execution order with more reasonable reason.
* Implemented `JCS_TweenPathAction` component.

### 2020-03-30

* Fixed camera position offset logic with hard track in both 2D/3D camera.

### 2020-03-29

* Implemented zoom in/out on mobile device.

### 2020-03-28

* Renamed some of the weird API naming in system module.
* You can now zoom in/out with touches in mobile device.
* Implemented touch event compatible with mouse event.

### 2020-03-26

* Organized `Resources` folder for exporting.

### 2020-03-25

* Fixed down compatibility to Unity version `5.3.3`.
* Changed project structure to have all the 3rd party dependencies on the 
 root of the `Assets` folder.

### 2020-02-29

* Added sound settings for `JCS_2DSlideScreenCamera`.

### 2020-02-28

* Added mobile slide GUI feature.

### 2020-01-27

* Update project's Unity Engine version to 2019.3.3f1.
* Fixed `JCS_3DCamera` out of range issue.

### 2020-02-24

* Clean up some scripts from `GameObject` module.

### 2020-01-31

* Fixed ensure all class inherit all parent class for settings.
* Fixed ensure all class inherit all parent class for managers.

### 2020-01-30

* Update package exporter to version `1.0.3`.
* Re-design debug module.

### 2020-01-19

* Update project's Unity Engine version to 2019.2.18f1.

### 2020-01-07

* Changed Unity scripting's default foramt.

### 2019-11-14

* Added PEY game logo to Games made.

### 2019-11-01

* Minor fixed from `Update` to `FixedUpdate` for `JCS_3DConstWaveEffect`.

### 2019-10-16

* Implemented `JCS_JSONGameData` component.
* Update project unity engine version to 2019.2.9f1.

### 2019-09-24

* Implemented JCS_IAPManager for In-App-Purchase system.

### 2019-09-23

* Update project unity engine version to 2019.2.6f1.
* Started integrating In-App-Purchase (IAP) system.

### 2019-09-12

* Update project unity engine version to 2019.2.4f1.
* Update dependency => `Log Viewer` to version `1.8`.

### 2019-08-12

* Fixed ensure frame text issue.

### 2019-08-05

* Fixed callback for tweener implementation issue.
* Fixed weird callback logic in `JCS_TransformTweener`.

### 2019-08-01

* Update project unity engine version to 2019.2.0f1.

### 2019-07-29

* Update project unity engine version to 2019.1.12f1.

### 2019-07-25

* Removed old documents from project root directory => `./doc`.

### 2019-07-24

* Update project unity engine version to 2019.1.11f1.

### 2019-07-23

* Implemented new component `JCS_Marquee`.

### 2019-07-18

* Implemented new component `JCS_TextAnimation`.

### 2019-07-17

* Update project unity engine version to 2019.1.10f1.
* Implemented new component `JCS_TextDeltaNumber`.

### 2019-07-12

* Implemented new component `JCS_TextTimer`.

### 2019-07-05

* Add round up option for sprite timer.
* Update project unity engine version to 2019.1.9f1.

### 2019-06-22

* Update project unity engine version to 2019.1.7f1.

### 2019-05-06

* Update project unity engine version to 2019.1.1f1.

### 2019-04-17

* Update project unity engine version to 2019.1.0f2.

### 2019-04-01

* Update project unity engine version to 2018.3.11f1.
* Complete tooltips.

### 2019-03-22

* Add missing tooltips.
* Update project unity engine version to 2018.3.9f1.

### 2019-03-16

* Format code with JCSUnity's standards.

### 2019-03-14

* Clean up some code and polished some classes' description.

### 2019-03-09

* Update project unity engine version to 2018.3.8f1.
* Complete Action/Freeze module's components.

### 2019-02-28

* Format code, supply missing tooltips and function descriptions.

### 2019-02-27

* Release JCS_ToggleButton's getter/setter and make some improvements with more reasonable function calls.

### 2019-02-25

* Fixed travis CI by removing `rvm get stable` command.
* Update travis CI with testing unity version => 2018.2.12f1.

### 2019-02-23

* Supply missing tooltips and function descriptions.
* Fixed some class descriptions.

### 2019-02-18

* Format action modules, and supply tooltips.

### 2019-02-16

* Update project unity engine version to 2018.3.6f1.
* Update some tooltips and class descriptions.

### 2019-02-09

* Update project unity engine version to 2018.3.5f1.

### 2019-02-06

* Organize project with `features` directory.

### 2019-02-05

* Update tooltips' typo.

### 2019-02-04

* Remove `JCSUnity_PE` and officially use `UnityPackageExporter` instead.

### 2019-02-02

* Update project unity engine version to 2018.3.4f1.

### 2019-01-27

* Update project unity engine version to 2018.3.3f1.

### 2019-01-25

* Fixed tooltips for particle module.

### 2019-01-20

* Update tooltips and fixed typo.

### 2019-01-18

* Update tooltips for better description.

### 2019-01-17

* Release some api calls.
* Organize code.
* Polish tooltips.

### 2019-01-16

* Fixed minor tooltips and section's format.

### 2019-01-13

* Update some modules' description.

### 2019-01-11

* Fixed classes' description.

### 2019-01-10

* Fixed enum module's description.

### 2019-01-09

* Remove trailing empty line from multiple files.

### 2019-01-07

* Organize code and fixed classes' description.

### 2019-01-06

* Add back and forth times => JCS_DestroyAnimBackForthEvent.cs.

### 2019-01-01

* Add header splitter, organize code.

### 2018-12-27

* Update some classes' description.
* Organized legacy code, components, etc.

### 2018-12-20

* Fixed some typo, class descriptions and variables descriptions.

### 2018-12-18

* Fixed some typo and class descriptions.

### 2018-12-16

* Fixed some of the APIs' getter/setter.

### 2018-12-14

* Update project unity engine version to 2018.3.0f2.
* Update project version to 1.8.7.
* Fixed all warnings from updating to unity engine version to 2018.3.0f2.

### 2018-12-13

* Update `Log Viewr' library/dependency to self-patch version 1.6.

### 2018-12-01

* Update project unity engine version to 2018.2.18f1.

### 2018-11-27

* Start travis continuous integration service => .travis.yml file.

### 2018-11-23

* Update project unity engine version to 2018.2.17f1.

### 2018-11-16

* Update project unity engine version to 2018.2.16f1.

### 2018-11-11

* Update project unity engine version to 2018.2.15f1.

### 2018-11-08

* Fixed several demo scenes.
* Remove deprecated module component that Unity declare as deprecated.

### 2018-10-29

* Update project unity engine version to 2018.2.14f1.

### 2018-10-21

* Update project unity engine version to 2018.2.13f1.

### 2018-10-13

* Update project unity engine version to 2018.2.12f1.

### 2018-10-10

* Move realted section into one section in the readme file.
* Add the manual/scripting api section into readme file.

### 2018-10-09

* Update project unity engine version to 2018.2.11f1.

### 2018-10-01

* Rename '2D Game' to just '2D' with parent folder 'Managers'.
* Update project version to 1.8.5.
* Revert to version 1.8.3, and update these changes.
   - Rename In Game Log System => InGameLogSystem.
* Revert back to 1.8.5 but keep the version 1.8.3's changes.

### 2018-09-27

* Update project unity engine version to 2018.2.10f1.

### 2018-09-25

* Add option disable sound when window not focus in JCS_SoundManager.
* Fixed JCS_PositionCastAction not compatible with resizable screen/window functionalities' issue.
* Fixed JCS_ScaleEffect and JCS_SlideEffect not part of the UI issue, missing the TEXT component.

### 2018-09-24

* Update project Unity Engine version to 2018.2.9f1.
* JCS_TextButtonEffect implemented, for button that is only the text without sprite.

### 2018-09-23

* Test android build.
* Fixed all warnings when build mobile phone version's application/executable.

### 2018-09-22

* Remove 'JCS' folder name's prefix in the JCSUnity_Resources.
* Update project version to 1.8.3.
* Add 'Standalone File Browser' as project dependency.
  => https://github.com/gkngkc/UnityStandaloneFileBrowser
* Make screen type handle to individual scene.

### 2018-09-21

* Implement DestroyImmediateAllTypeObjectInScene util function in JCS_Uility module.
* Increase the preformance of the removing pause action from the pause manager. 
The time complexity increase from O(n^2) to O(n).

### 2018-09-20

* Add test option and null sprite after done playing animation option to JCS_2DAnimation module.
* Add clear all undo/redo comps and all undo/redo history, in JCS_UndoRedoSystem and 
JCS_UndoRedoComponent class for undo redo module and easier function API call.
* Add there is undo redo history check API.
* Rename undo/redo system's API function call => start/stop recording to start/stop recording all.
* Organize code and add class desciption if the class do not have one.

### 2018-09-14

* Update Unity Engine version to 2018.2.8f1.
* Rename from JCS_AspectScreenPanel to JCS_ResizableScreenPanel, I think this is 
the proper naming, using 'resizable' than the word 'aspect'.
* Remove JCS_AspectScreen and move the functionalities to JCS_ScreenManager.

### 2018-09-12

* Fixed roll button selector acting weird when under different resolution issue.
* Add resize screen option everytime a new scene loaded to JCS_ScreenSettings.
* Implement resize screen on particular scene functionalities in JCS_ScreenManager.
* Resizable window/screen functionalities implemented, finally!
* Update project version to 1.8.1.

### 2018-09-10

* Fixed JCS_ButtonPointer compatible with resizable screen/window.

### 2018-09-09

* Implement JCS_ApplicationSettings for deeper application control.
* Update execution order with resizable screen functionalities.
* Implement JCS_ScreenSettings for storing screen setting over scene.

### 2018-09-08

* Implemented resizable screen module with JCS_ApsectScreen and JCS_AspectScreenPanel implemented.
* Implemented OnResizeScreen callback for resizable module in all camera module.
* JCS_ScreenManager implemented for resizable screen task handle.
* JCS_ScreenSettings implemented for screen related settings storage.

### 2018-09-06

* Fixed and make compatible with new Slide effect component in JCS_SequencePanel.
* Deprecated Utility function => JCS_MouseOverGUI. This function isn't work well 
enough with resizable screen/window.
* Make compatiable with resizable screen/window and add audo add event trigger event 
to event trigger system.
* Make tween panel compatible with resizable screen/window.
* JCS_InputField implemented for better input field handle.
* Make compatible with resizable screen/window with JCS_ScaleEffect, basically the 
same issue as JCS_SlideEffect. The solution are the same to both of the component.
* Update project versiont to 1.7.9.

### 2018-09-05

* Add attach/detach all child util functions.
* JCS_AnchorPresetsType enum implemented.
* Update scripting manual with JCS_AnchorPresetsType file description.
* Fixed all Unity object goes weird issue when having Unity defined UI as parent or
child transform as another Unity defined UI.
* Add merge list utilty function in JCS_Utility module.
* Use force detach children instead of normal detach children in JCS_PanelChild module.

### 2018-09-04

* Remove all the weird testing key with test component with key options in there.
* Instead set resolution and make aspect screen in Update we do it in LateUpdate function call.
* Add fix text by font size and fix text by scale options in JCS_PanelRoot component 
work with JCS_PanelChild component.

### 2018-09-03

* JCS_AspectScreen implemented for aspect ratio window/screen functionalities.
* Fix text component by scaling the size when doing the aspect ratio UI in JCS_PanelChild module.
* Add GCD -greatest common factor math/util function.

### 2018-09-02

* Upgrade Unity Engine to 2018.2.6f1.

### 2018-08-29

* Add alt, ctrl and shift key combination enum to JCS_KeyCombination.
* Add alt, ctrl, and shit key combination input.
* Update project version to 1.7.7.

### 2018-08-28

* Add create undo redo system editor function in JCSUnity Editor window's GUI section.
* Update JCSUntiy source url in editor properties .ini file.
* Add test valid number string utility function.
* Add game done initialize flag in JCS_GameManager.
* Add stop/start recording undo/redo action in both JCS_UndoRedoSystem and JCS_UndoRedoComponent.
* Make record prev data public function in JCS_UndoRedoComponent script, for any data
that developer want to record undo/redo's data manually.
* Add after game is done initialize callback in JCS_GameManager, manager's module.
* Make record previous data in JCS_UndoRedoComponent to after game is initialize callback.
* JCS_KeyWith implemented for key combination data struct, just to hold alt, ctrl, 
shift combination key info.
* JCS_UIComponentData interface for root of all UI component data.
* Implemented focus after undo/redo functionalities.

### 2018-08-27

* Undo Redo system implemented.
  -> JCS_UndoRedoSystem
  -> JCS_UndoRedoComponent
* Undo Redo system demo scene added.
### 2018-08-26

* Add GUI struct data script.
  -> JCS_DropdownData
  -> JCS_InputFieldData
  -> JCS_ScrollbarData
  -> JCS_SliderData
  -> JCS_ToggleData
* Add Ctrl, Alt, Shift related key functions.

### 2018-08-24

* Add toggle is on and off callback in JCS_Toggle component.

### 2018-08-23

* Update Unity Engine version to 2018.2.4f1.
* Add set interactable callback in JCS_Button, GUI module.
* Make compatible with button interactable by setting the alph
 the same but keep the rest of the colors to the toggle color.
* Update Unity Engine version to 2018.2.5f1.
 
### 2018-08-22

* JCS_Toggle implemented to better toggle UI/UX.
* JCS_ToggleSign implemented for JCS_Toggle's sign/button.

### 2018-08-21

* Add add dropdown option to JCS_Dropdown utility function to JCS_GUIUtil module.

### 2018-08-18

* Ignore and Remove build info file from 'Assets/StreamingAssets/' directory.
* Add build_info.txt text file to the JCSUnity_PEs' ignore list.
* Support one full path for XML and binary save load module.

### 2018-08-17

* Add JCS_OnMouseDrag function to check if the current mouse button is in the dragging action.
* Remove 'JCS_InputType' enum, is a mistake script by moving from other test module project.
* Update project/development version to 1.7.4.

### 2018-08-16

* Update IO module's scripting manual.

### 2018-08-15

* JCS_IO implemented for Input/Output utilities.
* Add JCS_Dropdown utility functions in JCS_GUIUtil module.

### 2018-08-14

* JCS_Dropdown implmeneted for better dropdown UI handle.
* Add more dropdown util function in JCS_GUIUtil module.

### 2018-08-13

* Add test option to JCS_3DLiquidBar.

### 2018-08-11

* JCS_GradientText implemented for making the text gradient effect.
* Support load image as texture in image loader module.

### 2018-08-09

* Change JCS_2DAnimation's displays variable from normal array to list data structure.

### 2018-08-08

* Add active/play on awake/loop variable to JCS_2DOrderAnimPlayer.
* Add 'displays' variable in JCS_2DAnimation module for multiple 
 JCS_UnityObject object displaying the same sprite.

### 2018-08-07

* Add test key option on few effect modules/components.

### 2018-08-06

* Add boolean check for if the specific joystick plugged-in util input function.
* Fixed slide input on focus changed the delta value issue.
* Add the test key option in JCS_GameManager.

### 2018-08-05

* Show joystick names in the JCS_InputSettings' inspector.
* Add check all keys pressed/up/down and either key pressed/up/down.
* Add check all keys pressed/up/down and either key pressed/up/down with joystick too.

### 2018-08-02

* Make legacy script for deprecated module => JCS_VideoPlayer component.
* Update Unity Engine version to => 2018.2.1f1.

### 2018-07-30

* Implement remove empty slot to normal array in Util module.
* Audio loader module implemented.
* Video loader module implemented.
* Update scripting manual for audio/video loader.

### 2018-07-23

* Add GUI util, for dropdown set to value function.
* Improve the usage of the dropdown set value function.

### 2018-07-22

* Add test key in JCS_DeltaNumber script/component.
* Make current project version to 1.7.1.

### 2018-07-20

* Enable loop to all BGM player object in each scenes.
* Fixed gamepad hit twice at the first key issue.
* BGM play prefab, enable loop option as default.

### 2018-07-19

* Change the project location, make sure the project does not break with build info confirm.
* Remove root scripts trailing white spaces.

### 2018-07-16

* Make 100 pixel per unit as default for Image Loader.
* JCS_GUIUtil for GUI utilities functions implemented.
* Add missing class description manual.
  -> JCS_GUIUtil
  -> JCS_Logger

### 2018-07-15

* Rename 'JCS_Buttons' folder to 'Button' for styling improvement.
* Add 'Button' component getter => ButtonComp at file 'JCS_Button'.
* Update scripting manual folder naming, JCS_Buttons => Button.

### 2018-07-12

* Fixed missing BGM player object in the GUI demo scene.
* Add missing mobile input related classes descriptions.

### 2018-07-11

* Add truncate float function to Math module.

### 2018-07-10

* Update class description and null ref check for spawned GUI objects for JCS_PanelChild.
* Add get selected value for Dropdown object, add UI utility module?
* Update default resource prefabs object with order layer of 5 instead of 100.
* Update project version to 1.6.9.

### 2018-07-04

* Implement BGM player for all scenes.
* Implement BGM not change between scene functionality.
* Implement PS4 gamepad to JCSUnity framework Input module.

### 2018-06-29

* Implement Visible On zero functionality on JCS_DeltaNumber component.
* Implement alignment for JCS_DeltaNumber component.

### 2018-06-27

* Update doc version number to current version number.
* JCS_BinGameData interface/module implemented.
* Update scripting manual with save load module implementation.

### 2018-06-20

* Make default empty array instead of null pointer in ReadSceneName component/module.
* Remove deprecated component from resource prefabs => GUI Layer.

### 2018-06-09

* Release version 1.6.5 for huge upgrade the package to Unity version 2018.1.3f1.
* Upgrade package JCSUnity_PE for JCSUnity project management.
* Update project version to 1.6.7.
* Fixed editor window initialize issue in Unity version 2018.1.3f1.

### 2018-06-08

* GUI resize module make compatible to Unity version to 2017.4.2f2.
* Update resize UI prefab default settings/configs.
* Remove deprecated 'README.txt' file.
* Add 'version_info.txt' for future use to record all version support.
* Update Unity version to the project to 2018.1.3f1.

### 2018-06-05

* Fixed JCS_VideoPlayer compatible with browser and WebGL build. It still have to be implement though.

### 2018-03-24

* Add link to game - sugar sleuths.

### 2018-02-17

* Rename movie/clip file to ASCII only for some versiont control, not allow UTF-8 compatbile issue.

### 2018-02-06

* Update Unity version to 2017.3.0f3.
* `Log Viewer' plugin seems no longer maintain, consider remove the plugin.

### 2018-01-04

* Add ignore pause check boolean and open to the inspector level.
* Add ignore pause option param to any key state input.

### 2018-01-02

* Gamepad version of slide screen button implemented.
* Add test key option to JCS_2DSlideScreenCamera.

### 2017-12-20

* Refactor clear keymap buffer for joystick into properly named function.
* Add tutorials section to readme file.

### 2017-12-19

* Fixed input module's release key up and key down by adding another keymap.

### 2017-12-17

* Fixed dialogue system will still play sound while dialouge does not show up corresponding UI.

### 2017-12-14

* Update compatible hover check.
* Add global sound effect to button sound effect script.
* Update dialogue system with default using global sound effect.
* Update project version to 1.6.5.

### 2017-12-09

* Add defense programming to 2d dynamic scene manager checking adding object to provided
order layer index.
* Add local position option to certain 3D action module.
* Cancel direction linking to duplex linking for button and button selection.
* Button selection group make compatible with mouse/PC by adding hover functionalities 
with Unity's built-in component 'EventTrigger'.

### 2017-12-05

* Add game pad button option with playing the sound with global sound player.

### 2017-11-21

* Implement sound for dialogue system using global sound player.
* Implement randomize value at start for JCS_3DGoStraightAction.

### 2017-11-17

* Add defense programming to 'JCS_2DDynamicSceneManager' class, missing prompt of the
set object. transform.

### 2017-11-13

* Macro define for free version of JCSUnity.
* Remove build warnings.
* Start draft version 1.6.3.
* Update readme file with 'how to use it?' and 'notice' sections.

### 2017-11-12

* JCS_PredictCamera implemented, now camera can have total of 3 dimensionals and 27 
directions' prediction.

### 2017-11-10

* Detail comment for 3D throw action with physics logic.

### 2017-11-09

* Bug fixed to 3D throw action, comparing from distance to displacement in free fall/accelerate formula.

### 2017-11-01

* JCS_ButtonSelectionGroup callback invoke with defense programming.
* JCS_ButtonSelection provide link direction from/to JCS_Button.
* JCS_DialogueScript force get dialogue system well getting it.
* Change dialogue system prefab because button selection link direction default is the opposite direction.

### 2017-10-31

* JCS_GamePadAnimationHandler implemented.
* JCS_2DAnimator header naming error fixed.

### 2017-10-30

* Add 'JCS_OnClickCallback' abstract function for JCS_Button class. Restrict all 
button sub-class follows this method/standard, in order to reduce system duplicate 
code.

### 2017-10-27

* JCS_HideDialogueGamePadButton implemented.
* JCS_ShowDialogueGamePadButton implemented.
* JCS_ActivePanelGamePadButton implemented.
* JCS_DeactivePanelGamePadButton implemented.
* JCS_ExitAppGamePadButton implemented.
* JCS_LoadSceneGamePadButton implemented.
* JCS_OpenURLGamePadButton implemented.
* JCS_PauseGameGamePadButton implemented.
* JCS_ToggleGamePadButton implemented.
* JCS_UnPauseGamePadButton implemented.
* JCS_ExitAppOnlineGamePadButton implemented.
* JCS_SwitchServerGamePadButton implemented.
* JCS_ButtonSelectionGroupController add JCS_ButtonSelectionGroup accessibility.

### 2017-10-24

* Add JCSUnity_PE for managing packages for my framework.

### 2017-10-23

* JCS_WhiteScreen with test key.
* add auto hover to dialogue system module.
* JCS_EchoGamePadButton implemented.
* JCS_ButtonSelectionGroup with audio settings now.
* JCS_GamePadButton with audio settings now.
* JCS_ButtonSoundEffect implement JCS_SoundMethod.
* JCS_SoundMethod enum added.
* JCS_SoundPlayer with play sound depends on action.
* JCS_RollBtnSelector bug fixed and compatible with base class JCS_Button class.
* Create Tool section for JCSUnity easy create project settings.

### 2017-10-22

* Update JCSUnity Editor window for better order and with some proper GUI label help understand the framework itself.
* Add accessibility to JCS_TweenPanel classs.
* Add functionality with reset all selections on 'OnEnable' function occurs.
* JCS_ButtonSelectionGroupController with option of ignore the game pause trigger for all input type.
* Add param 'ignorePause' to ignore game pause check for all input functions.

### 2017-10-21

* Rename Conversation_Dialogue to JCS_DialogueSystem for proper naming issue.
* Add comment to JCS_DialogueSystem.
* Make dialogue system game pad compatible.

### 2017-10-19

* Loop through the array and index in array range check functions added for utility layer.
* JCS_ButtonSelection and JCS_ButtonSelectionGroup, make compatible with 'skip' functionality.
* JCS_ButtonSelectionGroup with proper function naming design, mainly pural issue.
* Update JCS_ButtonSelection, JCS_ButtonSelectionGroup and JCS_ButtonSelectionGroupController 
with full control which is the 4 directions/2 dimensional control.
* Add self as button for Button selection.

### 2017-10-18

* Make JCS_Button callback compatible with Game pad system.
* Change default button selection group controller's active variables to true.
* JCS_GamePadButtonUIHandler implemented.
* Add Dialogue system on readme file.
* Add JCS_GamePadSpriteHandler for all kind of sprite object handle, now the sprite 
can be change by the joystick connection.
* Bug fixed, JCS_ButtonSelection 'mActive' to public variable 'Active'.
* JCS_Button with separate initialize function call.

### 2017-10-14

* Make JCS_Button compatible with keyboard and gamepad input buffer.
* Make JCS_GamePadButton compatible with keyboard input.
* Update JCS_3DThrowAction for throw action movement.

### 2017-10-13

* Change black screen and slide black screen default setting from resource object.

### 2017-10-11

* Change movie file for smaller file size.
* Remove webcam screen shots.

### 2017-10-10

* Utility tool wrong naming fixed.
* Sprite Timer out of range for level design, extends it.
* Remove UI error prompting, need to restrucutre the UI layer.
* Selection support unity callback.
* Random utility support include/cover funcation call.

### 2017-10-09

* JCS_FadeObject bug fixed with fade effect behind.
* JCS_FadeObject with better callback function format/naming.

### 2017-10-08

* Update JCS_ButtonSelection with callback.
* Instead of one effect replace with multiple effects for JCS_ButtonSelection.
* Sprite timer conflict with unity object time issue bug fixed.

### 2017-10-07

* Add JCS_GamePadButton base button class for Gamepad interaction.
* JCS_Input with more functional key input naming and readability.
* Now the framework handle multiple controller input with easy function call.
* Add Gam Pad key down and key up check.
* JCS_ButtonSelectionGroup implemented.
* JCS_ButtonSelection implemented.
* JCS_ButtonSelectionGroupController implemented.
* JCS_InputController implemented with default input manager settings.

### 2017-10-05

* Sprite Timer compatible with unity object.

### 2017-10-03

* Add 'NONE' as a default gamepad keycode in JCS_Input script.
* Add handy function enum size in utility layer.

### 2017-10-01

* Add basic two-sided surface shader with transparency.
* Add test key to JCS_FadeObject.
* JCS_FadeObject initial visible boolean bug fixed.

### 2017-09-30

* Audio listener null check bug fixed.
* Add IsVisible variable for base dialogue object.

### 2017-09-29

* Add dialogue object for active and deactive panel button.
* Add zoom function for 2D camera.
* Add Sprite Timer with callback when time is up.

### 2017-09-22

* JCS_TransformTweener update with target and self track.
* Add local and global switch for JCS_TransformTweener.
* Rotate action add with local track.

### 2017-09-18

* JCS_RevolutioAction implemented.

### 2017-09-17

* Include 'UI-Polygon' library made by Credit CiaccoDavide.

### 2017-09-16

* Add circle position function for x, y, z axis.
* Add radian and degree conversion.

### 2017-09-12

* Resolution fix with logo scene.
* Create tween panel added into the JCSUnity editor window.

### 2017-09-11

* Add fade screen to UI manager, now the game are enable the simple focus and un-focus effect.
* Add randomize const wave effect.
* JCS_TransformTweener continue tween provide all three transform type now, so 
developers can tween between position/rotation/scale for now on.
* Const wave effect support 3 dimentional, and using Untiy Object as the base class 
which mean the effect can be use by any Unity object layer.
* JCS_RandomTweenerAction implemented.
* Downgrade resources's screen panel object, in order to save a bit of runtime memory
usage and performance.
* Add value offset to JCS_TransformTweener.

### 2017-09-10

* Fix wrong naming/typo.

### 2017-09-09

* PacketLostPreventer add the timer in order not to send the packet to often and 
save a bit of performance comparing 60 packets (frame rate) per second.

### 2017-09-07

* JCS_LookAtMouseAction implemented.

### 2017-09-05

* JCS_ActivePanelButton implemented.
* JCS_DeactivePanelButton implemented.
* JCS_SwitchServerButton implemented.
* JCS_SwitchServerButton implemented.
* JCS_ApplicationCloseSimulateSceneOnline scene added, for online exit client.

### 2017-09-04

* JCS_OpenURLButton implemented.

### 2017-09-03

* Rename PacketDecoder to JCS_DefaultPacketDecoder private same file name.
* Rename PacketEncoder to JCS_DefaultPacketEncoder prevent same file name.
* Out of range Bugs fixed with 'JCS_ServerRequestProcessor'.

### 2017-09-02

* JCS_PacketLostPreventer implemented for tracking down and solve the packet lost 
issue by using the UDP for network communicattion.

### 2017-09-01

* Add Asynchronous Datagram Socket for option in 'JCS_NetworkSettings'.
* JCS_UDPGameSocket implemented.

### 2017-08-30

* JCS_Canvas have ability to let other component add to the  resizable panel.
* Add asynchronous callback to socket.

### 2017-08-25

* JCS_ClientMode implemented.
* JCS_RecvPacketType implemented.
* JCS_SendPacketType implemented.
* Add more meaningful to JCS_DefaultClientHandler.
* JCS_PacketProcessor implemented for sample the network handle.
* Error handling for server packet/request.
* JCS_BinaryReader implemented for another framework 'JCSNetS'.

### 2017-08-24

* JCS_Client implemented.
* JCS_ServerRequestProcessor implemented.
* Add require component to 'JCS_NetworkSettings' for 'JCS_ServerRequestProcessor'.

### 2017-08-23

* JCS_PacketHandler implemented.
* JCS_DefaultClientHandler for demo use.
* JCS_Packet for standard byte array storage class.

### 2017-08-22

* Default buffer input and output mem does not set correctlly, settings fixed.
* Client handler logic error in Game Socekt.

### 2017-08-21

* Client handler implemented, so we could have our own packet/buffer handler.

### 2017-08-20

* Update JCS_VelocityInfo with 3 dimentional.
* Re-structure the networking module.
* JCS_Debug no longer need the input of the file.
* JCS_Logger implmeneted for Network module.

### 2017-07-23

* Start the side project JCS_UCG.
* JCS_CameraRenderer implemented.
* GrayScale shader implemented.
* Displacement shader implemented.

### 2017-06-17

* Fixed Item will go through the wall.
* JCS_2DAnimDisplayHolder implemented.
* JCS_ItemWall implemented.
* Fixed JCS_TransformTweener continue tween issue.

### 2017-06-04

* Add Hotkeys category for JCSUnity GUI.

### 2017-05-29

* JCS_ButtonPointer implemented.
* Update Unity version to 5.6.1f1.

### 2017-05-25

* Rename JCS_Cursor to JCS_3DCursor.
* JCS_2DCursor implemented.
* Add slope to the JCS_PositionPlatform.

### 2017-05-19

* JCS_DamageText performance improvement.

### 2017-05-16

* JCS_ClimbableManager implemented.
* JCS_IndieManager implemented.

### 2017-05-14

* Rename JCS_2DRandAnimController to JCS_2DRandAnimByTimeController.
* JCS_2DRandAnimByAnimDone implemented.

### 2017-05-11

* JCS_FreezeTransformAction implemented.
* JCS_FreezePositionAction implemented.
* JCS_FreezeRotationAction implemented.
* JCS_FreezeScaleAction implemented.
* JCS_RelativeFreezePositionAction implemented.

### 2017-05-10

* JCS_2DOrderAnimPlayer implemented.
* JCS_2DRandAnimController implemented.
* JCS_AdjustTimeTrigger implemented.

### 2017-05-09

* JCS_2DAnimMirror implemented.

### 2017-05-08

* JCS_2DAnimSequencePlayer implemented.

### 2017-05-05

* JCS_2DLadder and JCS_2DRope update with sprite control front and behind.
* JCS_OrderLayer reasonable variable and renamed.
* JCS_OrderLayerObject add multiple SpriteRenderer component as option.

### 2017-05-04

* JCS_DeltaNumber with clear up zero on the left function implemented.

### 2017-05-02

* Rename JCS_SpriteScore to JCS_DeltaNumber.
* JCS_PlatformSettings implemented.

### 2017-05-01

* JCS_RouteGuageSystem implemented.

### 2017-04-28

* JCS_AdvertismentManager implemented.
* JCS_RewardAdsButton implemented.

### 2017-04-24

* Bug fixed JCS_OrderLayer parallax effect jittering issue.
* Zero out the velocity when the camera stopped, bug fixed.
* Update JCS_SoundManager with play one shot of bgm.

### 2017-04-20

* Update JCS_SceneManager with more proper name variables.
* Update JCS_SoundManaer with 'SwitchBackgroundMusic' function.

### 2017-04-17

* Rename JCS_2DDestroyAnimEffect to JCS_3DDestroyAnimEffect.
* JCS_2DDestroyAnimEffect impelemented.
* JCS_TransformPool implemented.
* JCS_DestroySpawnEffect implemented.

### 2017-04-14

* JCS_INIFileReader implemented.
* editor.properties implemented.

### 2017-04-10

* JCS_2DParticleSystem rename to JCS_ParticleSystem.
* JCS_DestroyAnimEffect rename to JCS_2DDestroyAnimEffect.
* Update JCS_ParticleSystem so you could play one shot of particle in certain number of particle.
* JCS_DestroyParticleEndEvent implemented.
* JCS_DestroyParticleEffect implemented.
* JCS_Tweener rename to JCS_TransfromTweener.
* JCS_ColorTweener implemented.

### 2017-04-07

* JCS_3DDistanceTileAction implemented.
* JCS_2DGoStraightAction rename to JCS_3DGoStraightAction.

### 2017-03-24

* JCS_TimePanel implemented.

### 2017-03-13

* JCS_2DAnimation implemented.
* Change the file name old JCS_2DAnimator to JCS_I2DAnimator.
* JCS_2DAnimator implemnted.

### 2017-03-10

* JCS_SpriteScore implemented.
* JCS_HitListEvent update with self-destroy.
* JCS_SpriteTimer implemented.
* JCS_Utility implement DestroyAllTypeObjectInScene function.

### 2017-03-06

* JCS_BasicInitSpawner implemented.

### 2017-03-04

* Redefined JCS_DestroyAnimEffect.
* JCS_BasicWaveSpawner implemented.

### 2017-03-03

* Update JCS_VideoPlayer compatible with Andriod.
* Set JCS_2DCamera virtual function as default.

### 2017-02-24

* JCS_PauseAction implemented.
* JCS_PauseManager implemented.
* Modefied JCS_GameManager's GAME_PAUSE compatible to previous version of JCSUnity.
* Fit Screen Size some bugs fixed.
* JCS_ShowDialogueButton implemented.
* JCS_HideDialogueButton implemented.
* JCS_PauseGameButton implemented.
* JCS_UnPauseGameButton implemented.

### 2017-01-30

* Organize JCSUnity_EditorWindow class.

### 2017-01-22

* Add JCSUnity_EditorWindow for gui interface implementation.
* Add menu item serialize to JCSUnity 2d.
* Add menu item serialize to JCSUnity 3d.
* Add menu item About.

### 2017-01-06

* Add randomize duration at start time for tweener.

### 2016-12-13

* Now All damage text pool provide the specific audio clip function call option. 
Meaning all the skill you are designing can be use in different sound.
* For coding wise, after the "ApplyDamageText" function had the new variable 
 "AudioClip", pass it in to take the effect.
* Update JCS_2DParticleSytem with "start" and "stop" function

### 2016-12-22

* JCS_InvincibleTimeAction implemented.

### 2016-12-10

* Add scene portal layer.
* Setup the indie folder for managers and settings folder.
* JCS_PortalManager implemented.
* JCS_PortalSetting implemented.
* Add black slide screen while switch scene.

### 2016-12-04

* Setup the shader folder.
* "VertexLit with Z" added.
* FOW shader added.

### 2016-11-22

* JCS_GUIComponentLayer implemented.
* JCS_ItemDroppable update with more function.

### 2016-11-13

* JCS_TowardTarget implemented.
* JCS_DisableWithCertainRangeEvent implemented.

### 2016-11-05

* JCS_PushThrowAction implemented.
* OnLevelWasLoaded deprecated fixed so now version compatible with Unity 5.4 or higher.

### 2016-10-28

* JCS_SceneSettings implemented.
    ■ Fade in/out time for specific scene option.
* JCS_SceneManager effect and update cuz of the JCS_SceneSetting script.
* JCS_3DLookAction asymptotic rotate.

### 2016-10-26

* JCS_3DCamera smooth track function implemented.
* JCS_GUILiquidBar support depend on the alignment.
* JCS_3DLiquidBar support depend on the alignment.

### 2016-10-15

* Joystick implemented.
* JCS_InputSetting updated.

### 2016-10-10

* JCS_3DCamera zoom in/out feature implemented.
* JCS_FadeObject min/max alpha range implemented.

### 2016-10-03

* Project warnings clean up.

### 2016-09-29

* JCS_3DAnimator working on.
* JCS_3DBlendTreeAnimator working on.

### 2016-09-26

* JCS_Tweener updated.

### 2016-09-15

* Update item droppable.
* Multiple script comment.

### 2016-09-14

* Update Pathfinding Request, but there are still some bugs.

### 2016-09-05

* Modefied the name of JCS_GameErros into JCS_Debug, so there is no JCS_GameErros 
anymore. Plz use JCS_Debug instead.
* Export JCSUnity 1.3.7 package.
* JCS_2DAIStateSystem implemented.

### 2016-09-03

* Update project to version 5.4.0f3.
* Few bugs fixed compatiable to version 5.4.0f3.
    1) box colliders does not support negative scale

### 2016-08-31

* JCS_VideoPlayer implemented but only support -list file.
    -list
        .mov
* Application end scene implemented.

### 2016-08-28

* A star pathfinding implemented.
* README.txt file updated.
* Working on JCS_VideoPlayer.

### 2016-08-27

* Add image into dialogue system.
* Add name tag into dialogue system.

### 2016-08-26

* Dialogue System implemented.
* Dialogue Script system implemented.
* Workd on A* Pathfinding algorithm.

### 2016-08-24

* README.txt file updated.
* Working on Dialogue Scripting system.
* JCS_ScriptTester scene added.
* UtilitiesScene category added in scene folder.
* npc2100.cs test script updated.

### 2016-08-20

* Update damage text system, so now enemy and player can have different damage text set.
* README.txt file updated.

### 2016-08-19

* In Game Log System implemented.
* JCS_Settings interface implemented.
* Log system implemented into Boss Fight Game.
* JCS_Settings implemented.
* JCS_Managers implemented.

### 2016-08-18

* Damage Text stackover flow logic bug fixed.
* XML Save and Load implemented.
* Get back recover value.
* Health target cannot be damage while the game is over.
* BF_CashText implemented.

### 2016-08-11

* Limit the monster in the scene so it won't spawn too many monster.

### 2016-08-10

* JCS_2DBullet huge updated.
* Character animation fixed.

### 2016-08-06

* Sequence Shoot in Cursor.
* Push Skill implemented.
* Multiple attack implemented.

### 2016-08-01

* JCS_3DMouseMovement implemented.
* BF_HealthTarget implemented.
* JCS_LiquidBarInfo implemented.

### 2016-07-31

* IAP System using Soomla open source project.
* randomize the cash value.
* Item effect object implemented.

### 2016-07-29

* 目前最強的射擊武器.
* JCS_2DInitLookByTypeAction implemented.
* JCS_LiveObjectManager, JCS_2DLiveObjectManager implemented.
* 全圖殺怪.

### 2016-07-28

* JCS_3DLiquidBar implemented.
* JCS_HueController implemented.
* Update Level Text.

### 2016-07-27

* Update inspector controll within the GUI function.
    - JCS_SlideEffect

### 2016-07-26

* JCS_2DCamera smooth scroll implemeneted.
* Mana System game machnice implemented.
* JCS_LiquidBarText implemented.
* use JCS_LiquidBarText into game.

### 2016-07-24

* 打幣系統.
* End Game System implemented.
* BF_RainFaller impelemented.
* JCS_Reflect object more functional with position offset.

### 2016-07-23

* Collision Fixed.
* Cash System implemented.

### 2016-07-22

* Bug Fixed.
* BF_AutoAttacker implemented.
* Boss Fight game scene updated.

### 2016-07-20

* Fixed the collision overlapping by using FixedUpdate() function call from Unity Engine.

### 2016-07-19

* JCS_2DFlyActionIgnore.
* Fly Object ignore all platform in the scene now.
* Arrow Rain implemented.

### 2016-07-18

* AOE function implemented.
* JCS_2DReflectBulletAction.
* JCS_ButtonSoundEffect will have refuse sound when is not interactable.
* JCS_Button interactable function updated.
* JCS_2dFlyAction's basic function implemneted.

### 2016-07-17

* Player attack will wait all star hit the object before die.
* Boss Fight game scene updated.
* Memory Leak bug fixed.

### 2016-07-16

* Wave handler implmented.
* Game Level up implemented.
* JCS_2DDeadAction implmented.

### 2016-07-15

* Update Log Viewer Packages.
* JCS_3DDragDropObject implemented.
* Stars can shoot down each other.
* JCSUnity System updated.
* Player will attack according to the mouse click.
* Live Object animation functional.

### 2016-07-14

* Boss Fight wave handler implemented but not finish yet.
* JCS_GUIStarterScene implemented.
* JCS_2DWalkAction add the random walk speed function at initilzie time.
* JCS_2DJumpAction add the random jump force function at initilzie time.
* JCS_GameErros function update, so no need to enter type the name out anymore, 
 but the line number isnt update yet.
* BF_CharacterSpawnHandler implemented, now we can design our own character spawn positions.


### 2016-07-11

* Mad effect for JCS_WalkAction implemented.
* Start the design of the BF.

### 2016-07-10

* LiveObject can now attack player/each other.
* KnowBack algorithm implemented.
* Abiltiy Fromat provide attack and defense value.

### 2016-07-09

* JCS_2DWalkAction bug fixed.
* JCS_VelcotiyInfo more function implemented. MoveSpeed & RecordSpeed.
* JCS_LiveObject more function implemented.

### 2016-07-08

* JCS_2DCursorShootAction implemented.
* JCS_3DCursorShootAction implemented.
* JCS_2DJumpAction implemented.
* JCS_2DWalkAction implemented.
* JCS_VelocityInfo implemented.

### 2016-07-07

* JCS_2DLadder complete!

### 2016-07-05

* JCS_2DLadder bug fixed.
* Player improvement.
* JCS_Physics upgrade.
* JCSUnity Bugs fixed.

### 2016-07-03

* JCS_2DLadder big upgrade.
* JCS_Physics more colliding functions implemented.
* JCS_2DSideScrolldPlayer more functions implemented.

### 2016-07-01

* JCS_3DCamera updated.
* JCS_3DDemo scene make changes.

### 2016-06-30

* Complete Running Crush core system.
* Optimized JCSUnity a bit.
* Start design the new game Boss Fight.

### 2016-06-29

* Select a level before taking image from webcam.
* JCS_RollButtonSelector functional.

### 2016-06-28

* Liquid Bar for Running Crush functional.
* JCS_TweenerPanel new added.

### 2016-06-26

* Liquid Bar for Running Crush.
* SpriteMash liberaris implemented.

### 2016-06-23

* Update JCS_ButtonSoundEffect, now we does not need to add to trigger event manully.
* JCS_EnvironmentSoundPlayer added.
* JCS_Lightning effect.
* JCS_StaticLightning effect.

### 2016-06-22

* Update Detection area to multiple collider.
* JCS_Detect Area Action logic errors fixed.
* JCS_SlideScreenButton can now going direction with counts.
* JCS_SwitchSceneButton can now load scene limited base on platform type.
* RC_EffectItem, so the object will be effect by picking up the item!

### 2016-06-20

* JCS_RollBtnSelector implemented.
* JCS_SequenceSlidePanel implemented.
* JCS_SlideEffect updated.

### 2016-06-19

* JCS_RollBtnSelector implemented.
* JCS_RollSelectorButton implemented.
* JCS_SimpleTrackAction implemented.

### 2016-06-18

* System Updated.
* JCSUnity's GUI system bug fixed.
* Shop System small update.

### 2016-06-17

* Lib updated.
* JCS_SortingObject implemented.
* Shop and Lobby Scene small update.

### 2016-06-15

* Game System update.
* Gold Item.
* Save Load Bug fixed.
* Shop Scene.
* RC_Player can pick up, so player can gain gold.

### 2016-06-14

* Player Pointer.
* Revive Pointer.
* Save Load Function implemented.
* Modefied Tween to general.
* JCS_Item updated.

### 2016-06-13

* Tweener updated with position, rotation, scale.

### 2016-06-09

* RC_EffectObject implemented.
* RC_Goal implemented.
* Game System, so now the the game will act with what mode player chose.
* Save / Load Data implemented.

### 2016-06-08

* Tweener Transition implemented. (Using 3rd party library.)
    - http://gizma.com/easing/#quint1
    - https://github.com/PeterVuorela/Tweener

### 2016-06-07

* JCS_ScaleEffect implemented.
* JCS_ButtonSoundEffect implemented. (use this for any button, working with Unity's Event Trigger script.)
* JCS_SlideScreenButton implemented.
* Work on Example Project.
    - Lobby
    - Webcam
* Part of MO implemented into Running Crush. (Webcam part)

### 2016-06-06

* JCS_Cursor implemented. (Not complete yet.)
* Work on Example Project.
    - Logo
    - Lobby
    - Webcam
    - Game
    - Shop
* Cross Unity Object's Webcam. (Not sure is cross platform yet.)

### 2016-06-05

* Work on Example Project.

### 2016-06-04

* Perspective Camera detect weather the collider area render on the screen or not. (using Game Depth)

### 2016-05-31

* Full Screen Atk implemented.

### 2016-05-30

* Player had the better movement.
* After Jump Effect.

### 2016-05-29

* Particle System
* Weather Particle Interface
* Snow Flake (Weather Particle)
* Rain Drop (Weather Particle)

### 2016-05-28

* Fix Player Settings.

### 2016-05-27

* Swing Attack Action
* Absorb Effect (JCS_ShootAction)
* Sequence Shoot in order right now. (Logic Error fixed.)
* Speed Layer.

### 2016-05-26

* Ignore Item Tag
* Miss Damage Text Code.
* Sprite Auto Facing
* Ability Format working.
* Shoot in sequence
* Bug Fix
* Auto Detect and attack.

### 2016-05-25

* Deviation Throw
* Layer Friction Implemented.
* Abitilty Format interface.
* Hit and Damage Text(JCS_ApplyDamageTextAction)

### 2016-05-24

* Throw Action! (support for 3D right now but not tested yet.)
* Detect Area implemented!

### 2016-05-23

* Drop Item (only with sprite renderer layer.)
* GUI Button Slide.
* Pick up Item action
* Throw Action! (2D only for right now.)
* Update "JCS_PlayerManager".

### 2016-05-21

* Liquid Container(only on X axis!)

### 2016-05-15

* Season implemented. 

### 2016-05-14

* 2D&3D Tracking Action Implemented!
* Hit Event implemented!
* Created new category "Event"
* Shake Effect implemented!
* Cos-sine wave effect implemented!

### 2016-05-11

* Mix Damage Text Pool implemented!
* JCS_3DCamera implemented!

### 2016-05-10

* Damage Text Pool implemented!

### 2016-05-09

* Damage Text implemented!

### 2016-05-8

* Checkable feature implemented (when have ur mouse over a checkable 
    object it will appear a small description menu)
* Clean Code (there was duplicate code)

### 2016-05-07

* Resolution for all platform feature added (Tested! and it work!)
* Slide Input implemented (for touch pad)
* Application Manager managing the platform type form now on! 

### 2016-05-06

* Slide Switch Scene Function Completed
* Resolution for all platform feature added (Not Tested!)
* Bug Fix ("JCS_GameWindow" script)
* Bug Fix (Logog Scene)

### 2016-05-01

* Multi Track Camera 2D feature add
* Pair Key Panel and Button Panel up
* Scene Manager bug fixed

### 2016-04-30 

* Multi Track Camera 2D
* Sound Fade in/out while switching the scene
* Bug Fix (Logo Scene)
* Top Down Player added
