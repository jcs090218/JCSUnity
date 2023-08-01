# Change Log

All notable changes to this project will be documented in this file.

Check [Keep a Changelog](http://keepachangelog.com/) for recommendations on how to structure this file.


## 2.3.2 (Unreleased)
> Released N/A

* Add with key functions in the input module (20b12f996d4eff458555e3dcf58095130f76f98e)
* Use `UnityEvent` for toggle button (459466aa1d1be1a2909d82c28e90561df60acf8b)
* Add action button to accept any events (c2c463f34b55c91fcd51c26d55431139484c1b40)
* Move game data to app data instead (751a608a3a77e64011187cb84971d2b2c06d2978)
* Remove game pause options from input module (e4d6b1dc1b0e1745c6899d08261aa67954f297d1)
* Fix switch scene flag (b8f35d66dc776687a8a60e5b6eff5a27211168b6)
* Fix sound slider influencing the BGM sound player (b8f35d66dc776687a8a60e5b6eff5a27211168b6)

## 2.3.1
> Released Jul 16, 2023

* Add reload scene (0a4bbb742086737ab5316fd0e2c113e3b38d49f1)
* Allow 0 length sprite in 2D animation (f220b5713c37239cd8ab6253dd2d07d613f28af3)
* Fix 2D Animation SPF instead of FPS (3c2e44b96c7d7aba02fafb931063a182a1833dac)
* Add pause settings (742635f9176e55225e44b89bd5f76e682424b542)
* Fix black screen is stucked when pausing the game (c03e2a28ad1aca33e5824b60a4b39e953eff4a52)
* Fix scale time goes below 0 (70a1d394796c2f9de9b6fc8001563bb4a09bd872)
* Rely on [MyBox](https://github.com/Deadcows/MyBox) (dfebbb0910d52e5199cd9abda5ce6e97968afe74)
* Add by self option for rotate action (2e239b8a269e57ed0bca94557630224d34dcc668)
* Specify way to control frame rate in the editor (877351a8fb375c814867c30b49eb9ce01526abae)
* Add utility functions to choose one object from `list` or `array`. (089ea6f1f82b86d47a3579f5a169bc577492c8d8)
* Place menu item under category `Tools` (12c0220d20c797ef19b629c09a82e71ba20fb8ee)
* Add reset position for 3D distance tile action (02bc20b640c4fbb242185dbdea775f16db4e897e)
* Add feature to control time by keys (2f8c6a1c98b87baa61e3adb05371cd484622db9b)

## 2.3.0
> Released Sep 12, 2022

* Apply workaround for SafeArea bug.
* Fixed field of view miscalculation.
* Fixed incorrect resize UI when not using responsive type screen resize setting.
* Added functionality to load image through an URL.
* Moved all delegate declarations to it's own class.
* Renamed `GUI` folder to just `UI` instead.
* Shorten `SaveLoad` related components..
* Shorten module name from `Utilities` to just `Util`.
* Shorten class name from `JCS_Utility` to `JCS_Util`.
* Add new component `JCS_ToggleCanvasButton`
* Add new component `JCS_ShowCanvasButton`
* Add new component `JCS_CanvasCanvasButton`
* Fix unmatched file name `JCS_Manager` (1223fb41d90fe39bbfe3bcac76e8c59d112a616a)
* Reset damage text's transform for each display (70c0e0c9512544ac4106fbc66a9fef1f75bdbc74)
* Fix miscalculation around resolution for `JCS_GUILiquidBar` (c441c5668acf8da00d4ee8f43f3653f372588dc8)
* Add reload scene option for load scene module (0da7d0546de58d05ac25b39212026684361719f5)
* Add main canvas option in `Canvas` module (834fe2e4ae21cc5704675c56d30a17a41c37f28a)
* Fix effects on `JCS_Item` don't apply to Unity Object type (2dd9e97674e78f7a2587a3b1520952faa7cf718c)
* Add bounce friction to `JCS_OneJump` item effect (124726c736c7fbee352ec12315002f08210bb3cd)
* Remove annoying missing target message from `JCS_3DWalkAction` (feac70e8eaf1d7c100c33dfcf79a1bd75fe94897)
* Expand range limit on basic wave spawner (38a7c0c5571c66c94e18b24171340a283426e0bd)
* Expand range limit on basic init  spawner (87699dac6e5ec939d590f46a64798b7b83f5c4ac)
* Add new component `JCS_SoundSlider` (db619483ad014e57535bbbec1c6efce86c0cec7b)
* Remove ambiguous keyword `Score`, in `JCS_DeltaNumber`(d4056d56ed40e7ed3f08aea1d04e481cd23d0c95)
* Make `JCS_TextDeltaNumber` closer to `JCS_DeltaNumber` (5a6c38d44d9229531b6823c3d5924c0fb3d84ceb)
* Improve enable/disable component with time events, no longer just the behaviours (0314b03ea5862f9b1b4903f2a16025cf93859430)
* Add agent and obstacle check for `JCS_3DWalkAction` component (4973663dd0ca9c6a7ed2fd64ec395c64c1e122f7)
* Add `done` flag for enable/disable component with time events (33a27f21e0f7b7ad5709c05d24ec0cc13b488e90)
* Add callback options for visually full/empty liquid bars (8d56205721b4c669e73ce610da9e316de50b5569)
* Fix panel root from getting in parent, should restrain it from just one level parent (95529f449f04f071d84aa21259e140b50739476b)
* Add UI components to update slider value, `JCS_UpdateSliderButton` (ac5de4b0a387831c72df715184515a839211f3b4)
* Rename keyword `Game Pad` to `Gamepad` (adfc7d8eec2b0362ac3bba95089d7dd4bd123237)
* Add new component `JCS_SliderTextDisplay` to display slider's value (eb3ab41bbeaf828984d3627c9c115f4ed4bee3c7)
* Use text object interface to reduce duplicate code (760c6ac8c3b9a05623e64504614fb17f537a8837)
* Add new `UI/Slider` component `JCS_SliderInputField` (cd15404eff785615e7420f8642f338d329384ebe)

## 2.2.1
> Released Nov 1, 2021

* Add more document string.
* Add new component, `JCS_PageIndicators` for mobile dribbles action.
* Add new component, event `JCS_ActiveWithTime.`
* Add new component, event `JCS_InactiveWithTime.`
* Support alpha channel for component `JCS_AlphaObject`.
* Respect screen color from scene settings to scene manager.
* Add fit all option for screen module. (mobile)
* Replace screens' sprite instead the built-in `Background` UI sprite.
* Reveal timer variables for active and enable events.
* Escape `emailto` request for open URL.
* Implement new screen type, `MIXED` for responsive UI.
* Remove limitation on multiple canvas objects in the scene.
* Remove legacy designed component `JCS_IgnoreDialogueObject`.
* Clean up singleton game object module for better understanding.
* Add request permission capability to application manager.

## 2.2.0
> Released Aug 16, 2021

* Organize `.unityignore` files for exportation.
* Update `About JCSUnity` window layout.
* Add set page for slide camera.
* Add trimmed version.

## 2.1.4
> Released Aug 10, 2021

* Fix editor window with incorrect name.
* Save user settings using `EditorPref` utility class.
* Remove unused `xml` directory.
* Fix unused, total gamepad variable in input settings module.
* Place `Resources` folders to it's folder.

## 2.1.3
> Released Apr 18, 2021

* Fix compile errors from naming `Editors` to `Editor`.

## 2.1.2
> Released Apr 16, 2021

* Add byte array create function for image loader.
* Create new module `JCS_Path`.
* Add attribute `TextArea` to `JCS_LangData` component.
* Clean up code from `Network` module.
* Remove demo scripts, `ToOfficialSite`, `ToSettings`, etc.
* Update `PackageExporter` to version `1.0.4`.
* Update Unity Engine version to `2021.1.1f1`.
* Add support for multiple languages.
* Add fill slot function for array and list data type.
* Fix resize after attach UI component to resize canvas object.
* Update materials module's metadata.
* Improved unit testing for component `JCS_HopEffect`.
* Label the enumerator item for script `JCS_2D4Direction`.

## 2.1.1
> Released Dec 18, 2020

* Added safe area flag for iPhone X or above.
* Fixed stretches issue from safe area view.
* Fixed swipe over boundary point issue from UI module.
* Fixed slide input for mobile platforms.
* Clarify create directory from IO module.
* Use `persistentDataPath` instead of `dataPath` for data saving.
* Ignored all streaming assets for packaging.

## 2.1.0
> Released Nov 30, 2020

* Fixed panel root bug from `JCS_2DSlideScreenCamera` for incorrect swiping calculation.
* Simplify screen variables by merging `width` and `height` to just the `size`.
* Fixed screen space calculation for resizable screen.
* Swiping previous/next page are now calculated by percentage of the page space.
* Fixed logic `JCS_TransformTweener` for applying panel root.
* Fixed logic `JCS_2DSlideScreenCamera` for screen size.
* Added boundary for `JCS_2DSlideScreenCamera` in swiping behaviour.
* Fixed position differences when swiping screen UI.
* Filled default camera, microphone, location usage information.
* Changed from `Travis CI` to `GitHub Actions`.
* Updated project's Unity Engine version to `2020.1.2f1`.
* Implemented new component `JCS_TransformLinkedObject`.
* Implemented new component `JCS_TransformLinkedObjectController`.

## 2.0.7
> Released Aug 4, 2020

* Implemented streaming assets consistency over platforms using web request.
* Implemented streaming assets cache system for un-necessary loading.

## 2.0.6
> Released Jul 24, 2020

* Updated project's Unity Engine version to `2020.1.0f1`.

## 2.0.5
> Released Jul 10, 2020

* Implemented game data structure => `JCS_GameData`.
* Updated game settings for creating all necessary path/folders.
* Added struct for loaded image/sprite data => `JCS_LoadedSpriteData`.
* Fixed load all images last index logic issue from `JCS_Webcam`.
* Fixed load all images last index logic issue from `JCS_Camera`.
* Fixed image file not exist load for `JCS_ImageLoader`.
* Added more Camera screenshot image's API.
* Added more Webcam image's API.
* Updated webcam image utility functions.
* Implemented new component `JCS_ScreenshotButton`.
* Added remove from dir utility function.
* Added util function for webcam images.
* Added util function for screenshot images.
* Organized and added webcam settings to game settings.
* Updated webcam stopping logic.
* Fixed webcam texture remaining to the next scene issue.
* Fixed resize panel logic from `JCS_PanelRoot` component.

## 2.0.4
> Released Jun 30, 2020

* Updated `JCS_Webcam` for webcam module.
* Supplied `JCS_UnityObject` for generic Unity render object.
* Supplied `JCS_ColliderObject` for generic collider object.
* Organized source code for better document string.
* Removed unused variables in `JCS_3DCamera` component.
* Renamed framework scenes to a better standard.
* Implemented `JCS_OrderEvent` component.
* Updated to have tween panel shown ontop when active.
* Fixed minor logic error from testing component.
* Updated the create section from edtiro module for better UX.
* Differentiate the base panel and dialgoue panel for JCSUnity editor.
* Implemented switch BGM button component for GUI module.
* Organized sound player attachment default behaviour.
* Removed `JCS_2DShakeEffect`, please use `JCS_3DShakeEffect` instead.
* Upgraded 3D walk action from 3D AI module.
* Added panels section for GUI utility functions.
* Removed `GUI/Button/Dialogue` for repetitive functionalities with  other components/scripts.
* Implemented `JCS_ColliderType` enum for collider identification.
* Implemented `JCS_ColliderObject` for collider component management for single transform.

## 2.0.3
> Released May 7, 2020

* Added 3D walk action manager.
* Removed indie standard, use `others`/`2D`/`3D` instead.
* Fixed 3D walk action overlap destination search path logic error.
* Released more mix damage text pool API.
* Fixed `Pythagorean Theorem` calculation in math module.
* Fixed 3D Walk action AI with remaining distance calculation.
* Organized shader sources.
* Updated export settings so it no longer export frame test files.
* Updated the destory with time module with better flexible control.
* Upgraded the liquid bar module for better understanding API calls.
* Upgraded the damage text system for better default UX.
* Updated walk action with remaining distance accept variable.
* Now damage text system supports 3D space.
* Upgrade damage text system's user experience.
* Updated 3D walk action for AI module.
* Updated with detect touch count for touch event.
* Revert tween panel active/deactive logic.
* Added over GUI element check for utility module.
* Make `slide input` and `mobile mouse event` as optional to
 prevent multiple same scripts' execution.
* Add check scene utility function.
* Implemented `JCS_ValueTweener` compnent for tweener module.
* Implemented `JCS_3DLight` compnent for 3D module.
* Updated the 3D AI module in walk action.
* Added range include for float value.
* Added the path action for path finding module.
* Upgraded 3D camera revolution rotate action, is more stable and has the improvement of the UX.

## 2.0.2
> Released Apr 7, 2020

* Implemented `JCS_3DCameraPlayer` for character control relatives to camera, reference game `Monster Hunter`.
* Implemented `JCS_3DHintBubble` component to `GameObject` module.
* Implemented `JCS_SimplePathAction` component.
* Now tween panel support multiple transform tweeners.
* Implemented toggle panel default button.
* Added dispose callback for dialogue system.
* Updated the execution order with more reasonable reason.
* Implemented `JCS_TweenPathAction` component.
* Fixed camera position offset logic with hard track in both 2D/3D camera.
* Implemented zoom in/out on mobile device.
* Renamed some of the weird API naming in system module.
* You can now zoom in/out with touches in mobile device.
* Implemented touch event compatible with mouse event.
* Organized `Resources` folder for exporting.

## 2.0.1
> Released Mar 25, 2020

* Minor patch fix.

## 2.0.0
> Released Mar 25, 2020

* Fixed down compatibility to Unity version `5.3.3`.
* Changed project structure to have all the 3rd party dependencies on the root of the `Assets` folder.
* Added sound settings for `JCS_2DSlideScreenCamera`.
* Added mobile slide GUI feature.
* Update project's Unity Engine version to 2019.3.3f1.
* Fixed `JCS_3DCamera` out of range issue.
* Clean up some scripts from `GameObject` module.
* Fixed ensure all class inherit all parent class for settings.
* Fixed ensure all class inherit all parent class for managers.

## 1.9.5
> Released Jan 30, 2020

* Update package exporter to version `1.0.3`.
* Re-design debug module.
* Update project's Unity Engine version to 2019.2.18f1.

## 1.9.4
> Released Jan 7, 2020

* Changed Unity scripting's default foramt.
* Added PEY game logo to Games made.

## 1.9.3
> Released Nov 1, 2019

* Minor fixed from `Update` to `FixedUpdate` for `JCS_3DConstWaveEffect`.
* Implemented `JCS_JSONGameData` component.
* Update project unity engine version to 2019.2.9f1.
* Implemented JCS_IAPManager for In-App-Purchase system.
* Update project unity engine version to 2019.2.6f1.
* Started integrating In-App-Purchase (IAP) system.
* Update project unity engine version to 2019.2.4f1.
* Update dependency => `Log Viewer` to version `1.8`.
* Fixed ensure frame text issue.
* Fixed callback for tweener implementation issue.
* Fixed weird callback logic in `JCS_TransformTweener`.
* Update project unity engine version to 2019.2.0f1.
* Update project unity engine version to 2019.1.12f1.
* Removed old documents from project root directory => `./doc`.
* Update project unity engine version to 2019.1.11f1.
* Implemented new component `JCS_Marquee`.

## 1.9.1
> Released Jul 22, 2019

* Implemented new component `JCS_TextAnimation`.
* Update project unity engine version to 2019.1.10f1.
* Implemented new component `JCS_TextDeltaNumber`.
* Implemented new component `JCS_TextTimer`.
* Add round up option for sprite timer.
* Update project unity engine version to 2019.1.9f1.

## 1.8.7
> Released Apr 1, 2019

* Update project unity engine version to 2018.3.11f1.
* Complete tooltips.
* Add missing tooltips.
* Format code with JCSUnity's standards.
* Clean up some code and polished some classes' description.
* Complete Action/Freeze module's components.
* Format code, supply missing tooltips and function descriptions.
* Release JCS_ToggleButton's getter/setter and make some improvements with more reasonable function calls.
* Fixed travis CI by removing `rvm get stable` command.
* Supply missing tooltips and function descriptions.
* Fixed some class descriptions.
* Format action modules, and supply tooltips.
* Update some tooltips and class descriptions.
* Organize project with `features` directory.
* Update tooltips' typo.
* Remove `JCSUnity_PE` and officially use `UnityPackageExporter` instead.
* Fixed tooltips for particle module.
* Update tooltips and fixed typo.
* Update tooltips for better description.
* Release some api calls.
* Organize code.
* Polish tooltips.
* Fixed minor tooltips and section's format.
* Update some modules' description.
* Fixed classes' description.
* Fixed enum module's description.
* Remove trailing empty line from multiple files.
* Organize code and fixed classes' description.
* Add back and forth times => JCS_DestroyAnimBackForthEvent.cs.
* Add header splitter, organize code.
* Update some classes' description.
* Organized legacy code, components, etc.
* Fixed some typo, class descriptions and variables descriptions.
* Fixed some typo and class descriptions.
* Fixed some of the APIs' getter/setter.

## 1.8.5
> Released Dec 14, 2018

* Update project unity engine version to 2018.3.0f2.
* Fixed all warnings from updating to unity engine version to 2018.3.0f2.
* Update `Log Viewr' library/dependency to self-patch version 1.6.
* Start travis continuous integration service => .travis.yml file.
* Fixed several demo scenes.
* Remove deprecated module component that Unity declare as deprecated.
* Move realted section into one section in the readme file.
* Add the manual/scripting api section into readme file.

## 1.8.3
> Released Oct 1, 2018

* Rename '2D Game' to just '2D' with parent folder 'Managers'.
* Update project version to 1.8.5.
* Revert to version 1.8.3, and update these changes.
   - Rename In Game Log System => InGameLogSystem.
* Revert back to 1.8.5 but keep the version 1.8.3's changes.
* Update project unity engine version to 2018.2.10f1.
* Add option disable sound when window not focus in JCS_SoundManager.
* Fixed JCS_PositionCastAction not compatible with resizable screen/window functionalities' issue.
* Fixed JCS_ScaleEffect and JCS_SlideEffect not part of the UI issue, missing the TEXT component.
* Update project Unity Engine version to 2018.2.9f1.
* JCS_TextButtonEffect implemented, for button that is only the text without sprite.
* Test android build.
* Fixed all warnings when build mobile phone version's application/executable.
* Remove 'JCS' folder name's prefix in the JCSUnity_Resources.
* Update project version to 1.8.3.
* Add 'Standalone File Browser' as project dependency.
  => https://github.com/gkngkc/UnityStandaloneFileBrowser
* Make screen type handle to individual scene.

## 1.8.3
> Released Sep 22, 2018

* Implement DestroyImmediateAllTypeObjectInScene util function in JCS_Uility module.
* Increase the preformance of the removing pause action from the pause manager.
The time complexity increase from O(n^2) to O(n).
* Add test option and null sprite after done playing animation option to JCS_2DAnimation module.
* Add clear all undo/redo comps and all undo/redo history, in JCS_UndoRedoSystem and
JCS_UndoRedoComponent class for undo redo module and easier function API call.
* Add there is undo redo history check API.
* Rename undo/redo system's API function call => start/stop recording to start/stop recording all.
* Organize code and add class desciption if the class do not have one.
* Update Unity Engine version to 2018.2.8f1.
* Rename from JCS_AspectScreenPanel to JCS_ResizableScreenPanel, I think this is
the proper naming, using 'resizable' than the word 'aspect'.
* Remove JCS_AspectScreen and move the functionalities to JCS_ScreenManager.

## 1.7.9
> Released Sep 13, 2018

* Fixed roll button selector acting weird when under different resolution issue.
* Add resize screen option everytime a new scene loaded to JCS_ScreenSettings.
* Implement resize screen on particular scene functionalities in JCS_ScreenManager.
* Resizable window/screen functionalities implemented, finally!
* Fixed JCS_ButtonPointer compatible with resizable screen/window.
* Implement JCS_ApplicationSettings for deeper application control.
* Update execution order with resizable screen functionalities.
* Implement JCS_ScreenSettings for storing screen setting over scene.
* Implemented resizable screen module with JCS_ApsectScreen and JCS_AspectScreenPanel implemented.
* Implemented OnResizeScreen callback for resizable module in all camera module.
* JCS_ScreenManager implemented for resizable screen task handle.
* JCS_ScreenSettings implemented for screen related settings storage.

## 1.7.7
> Released Sep 6, 2018

* Fixed and make compatible with new Slide effect component in JCS_SequencePanel.
* Deprecated Utility function => JCS_MouseOverGUI. This function isn't work well
enough with resizable screen/window.
* Make compatiable with resizable screen/window and add audo add event trigger event
to event trigger system.
* Make tween panel compatible with resizable screen/window.
* JCS_InputField implemented for better input field handle.
* Make compatible with resizable screen/window with JCS_ScaleEffect, basically the
same issue as JCS_SlideEffect. The solution are the same to both of the component.
* Add attach/detach all child util functions.
* JCS_AnchorPresetsType enum implemented.
* Update scripting manual with JCS_AnchorPresetsType file description.
* Fixed all Unity object goes weird issue when having Unity defined UI as parent or
child transform as another Unity defined UI.
* Add merge list utilty function in JCS_Utility module.
* Use force detach children instead of normal detach children in JCS_PanelChild module.
* Remove all the weird testing key with test component with key options in there.
* Instead set resolution and make aspect screen in Update we do it in LateUpdate function call.
* Add fix text by font size and fix text by scale options in JCS_PanelRoot component
work with JCS_PanelChild component.
* JCS_AspectScreen implemented for aspect ratio window/screen functionalities.
* Fix text component by scaling the size when doing the aspect ratio UI in JCS_PanelChild module.
* Add GCD -greatest common factor math/util function.
* Upgrade Unity Engine to 2018.2.6f1.

## 1.7.4
> Released Aug 29, 2018

* Add alt, ctrl and shift key combination enum to JCS_KeyCombination.
* Add alt, ctrl, and shit key combination input.
* Update project version to 1.7.7.
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
* Undo Redo system implemented.
  -> JCS_UndoRedoSystem
  -> JCS_UndoRedoComponent
* Undo Redo system demo scene added.
* Add GUI struct data script.
  -> JCS_DropdownData
  -> JCS_InputFieldData
  -> JCS_ScrollbarData
  -> JCS_SliderData
  -> JCS_ToggleData
* Add Ctrl, Alt, Shift related key functions.
* Add toggle is on and off callback in JCS_Toggle component.
* Update Unity Engine version to 2018.2.4f1.
* Add set interactable callback in JCS_Button, GUI module.
* Make compatible with button interactable by setting the alph
 the same but keep the rest of the colors to the toggle color.
* Update Unity Engine version to 2018.2.5f1.
* JCS_Toggle implemented to better toggle UI/UX.
* JCS_ToggleSign implemented for JCS_Toggle's sign/button.
* Add add dropdown option to JCS_Dropdown utility function to JCS_GUIUtil module.
* Ignore and Remove build info file from 'Assets/StreamingAssets/' directory.
* Add build_info.txt text file to the JCSUnity_PEs' ignore list.
* Support one full path for XML and binary save load module.

## 1.7.1
> Released Aug 17, 2018

* Add JCS_OnMouseDrag function to check if the current mouse button is in the dragging action.
* Remove 'JCS_InputType' enum, is a mistake script by moving from other test module project.
* Update project/development version to 1.7.4.
* Update IO module's scripting manual.
* JCS_IO implemented for Input/Output utilities.
* Add JCS_Dropdown utility functions in JCS_GUIUtil module.
* JCS_Dropdown implmeneted for better dropdown UI handle.
* Add more dropdown util function in JCS_GUIUtil module.
* Add test option to JCS_3DLiquidBar.
* JCS_GradientText implemented for making the text gradient effect.
* Support load image as texture in image loader module.
* Change JCS_2DAnimation's displays variable from normal array to list data structure.
* Add active/play on awake/loop variable to JCS_2DOrderAnimPlayer.
* Add 'displays' variable in JCS_2DAnimation module for multiple
 JCS_UnityObject object displaying the same sprite.
* Add test key option on few effect modules/components.
* Add boolean check for if the specific joystick plugged-in util input function.
* Fixed slide input on focus changed the delta value issue.
* Add the test key option in JCS_GameManager.
* Show joystick names in the JCS_InputSettings' inspector.
* Add check all keys pressed/up/down and either key pressed/up/down.
* Add check all keys pressed/up/down and either key pressed/up/down with joystick too.
* Make legacy script for deprecated module => JCS_VideoPlayer component.
* Update Unity Engine version to => 2018.2.1f1.
* Implement remove empty slot to normal array in Util module.
* Audio loader module implemented.
* Video loader module implemented.
* Update scripting manual for audio/video loader.
* Add GUI util, for dropdown set to value function.
* Improve the usage of the dropdown set value function.
* Add test key in JCS_DeltaNumber script/component.
* Make current project version to 1.7.1.

## 1.6.9
> Released Jul 20, 2018

* Enable loop to all BGM player object in each scenes.
* Fixed gamepad hit twice at the first key issue.
* BGM play prefab, enable loop option as default.
* Change the project location, make sure the project does not break with build info confirm.
* Remove root scripts trailing white spaces.
* Make 100 pixel per unit as default for Image Loader.
* JCS_GUIUtil for GUI utilities functions implemented.
* Add missing class description manual.
  -> JCS_GUIUtil
  -> JCS_Logger
* Rename 'JCS_Buttons' folder to 'Button' for styling improvement.
* Add 'Button' component getter => ButtonComp at file 'JCS_Button'.
* Update scripting manual folder naming, JCS_Buttons => Button.
* Fixed missing BGM player object in the GUI demo scene.
* Add missing mobile input related classes descriptions.
* Add truncate float function to Math module.

## 1.6.7
> Released Jul 10, 2018

* Update class description and null ref check for spawned GUI objects for JCS_PanelChild.
* Add get selected value for Dropdown object, add UI utility module?
* Update default resource prefabs object with order layer of 5 instead of 100.
* Update project version to 1.6.9.
* Implement BGM player for all scenes.
* Implement BGM not change between scene functionality.
* Implement PS4 gamepad to JCSUnity framework Input module.
* Implement Visible On zero functionality on JCS_DeltaNumber component.
* Implement alignment for JCS_DeltaNumber component.
* Update doc version number to current version number.
* JCS_BinGameData interface/module implemented.
* Update scripting manual with save load module implementation.
* Make default empty array instead of null pointer in ReadSceneName component/module.
* Remove deprecated component from resource prefabs => GUI Layer.
* Release version 1.6.5 for huge upgrade the package to Unity version 2018.1.3f1.
* Upgrade package JCSUnity_PE for JCSUnity project management.
* Update project version to 1.6.7.
* Fixed editor window initialize issue in Unity version 2018.1.3f1.

## 1.6.5
> Released Jun 8, 2018

* GUI resize module make compatible to Unity version to 2017.4.2f2.
* Update resize UI prefab default settings/configs.
* Remove deprecated 'README.txt' file.
* Add 'version_info.txt' for future use to record all version support.
* Update Unity version to the project to 2018.1.3f1.
* Fixed JCS_VideoPlayer compatible with browser and WebGL build. It still have to be implement though.
* Add link to game - sugar sleuths.
* Rename movie/clip file to ASCII only for some versiont control, not allow UTF-8 compatbile issue.
* Update Unity version to 2017.3.0f3.
* `Log Viewer' plugin seems no longer maintain, consider remove the plugin.
* Add ignore pause check boolean and open to the inspector level.
* Add ignore pause option param to any key state input.
* Gamepad version of slide screen button implemented.
* Add test key option to JCS_2DSlideScreenCamera.
* Refactor clear keymap buffer for joystick into properly named function.
* Add tutorials section to readme file.
* Fixed input module's release key up and key down by adding another keymap.
* Fixed dialogue system will still play sound while dialouge does not show up corresponding UI.

## 1.6.3
> Released Dec 14, 2017

* Update compatible hover check.
* Add global sound effect to button sound effect script.
* Update dialogue system with default using global sound effect.
* Update project version to 1.6.5.
* Add defense programming to 2d dynamic scene manager checking adding object to provided
order layer index.
* Add local position option to certain 3D action module.
* Cancel direction linking to duplex linking for button and button selection.
* Button selection group make compatible with mouse/PC by adding hover functionalities
with Unity's built-in component 'EventTrigger'.
* Add game pad button option with playing the sound with global sound player.
* Implement sound for dialogue system using global sound player.
* Implement randomize value at start for JCS_3DGoStraightAction.

## 1.6.1
> Released Nov 13, 2017

* Add defense programming to 'JCS_2DDynamicSceneManager' class, missing prompt of the
set object. transform.
* Macro define for free version of JCSUnity.
* Remove build warnings.
* Start draft version 1.6.3.
* Update readme file with 'how to use it?' and 'notice' sections.
* JCS_PredictCamera implemented, now camera can have total of 3 dimensionals and 27
directions' prediction.
* Detail comment for 3D throw action with physics logic.
* Bug fixed to 3D throw action, comparing from distance to displacement in free fall/accelerate formula.
* JCS_ButtonSelectionGroup callback invoke with defense programming.
* JCS_ButtonSelection provide link direction from/to JCS_Button.
* JCS_DialogueScript force get dialogue system well getting it.
* Change dialogue system prefab because button selection link direction default is the opposite direction.
* JCS_GamePadAnimationHandler implemented.
* JCS_2DAnimator header naming error fixed.

## 1.5.9
> Released Oct 30, 2017

* Add 'JCS_OnClickCallback' abstract function for JCS_Button class. Restrict all
button sub-class follows this method/standard, in order to reduce system duplicate
code.
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
* Add JCSUnity_PE for managing packages for my framework.
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
* Update JCSUnity Editor window for better order and with some proper GUI label help understand the framework itself.
* Add accessibility to JCS_TweenPanel classs.
* Add functionality with reset all selections on 'OnEnable' function occurs.
* JCS_ButtonSelectionGroupController with option of ignore the game pause trigger for all input type.
* Add param 'ignorePause' to ignore game pause check for all input functions.

## 1.5.7
> Released Oct 22, 2017

* Rename Conversation_Dialogue to JCS_DialogueSystem for proper naming issue.
* Add comment to JCS_DialogueSystem.
* Make dialogue system game pad compatible.
* Loop through the array and index in array range check functions added for utility layer.
* JCS_ButtonSelection and JCS_ButtonSelectionGroup, make compatible with 'skip' functionality.
* JCS_ButtonSelectionGroup with proper function naming design, mainly pural issue.
* Update JCS_ButtonSelection, JCS_ButtonSelectionGroup and JCS_ButtonSelectionGroupController
with full control which is the 4 directions/2 dimensional control.
* Add self as button for Button selection.
* Make JCS_Button callback compatible with Game pad system.
* Change default button selection group controller's active variables to true.
* JCS_GamePadButtonUIHandler implemented.
* Add Dialogue system on readme file.
* Add JCS_GamePadSpriteHandler for all kind of sprite object handle, now the sprite
can be change by the joystick connection.
* Bug fixed, JCS_ButtonSelection 'mActive' to public variable 'Active'.
* JCS_Button with separate initialize function call.
* Make JCS_Button compatible with keyboard and gamepad input buffer.
* Make JCS_GamePadButton compatible with keyboard input.
* Update JCS_3DThrowAction for throw action movement.

## 1.5.5
> Released Oct 14, 2017

* Change black screen and slide black screen default setting from resource object.
* Change movie file for smaller file size.
* Remove webcam screen shots.
* Utility tool wrong naming fixed.
* Sprite Timer out of range for level design, extends it.
* Remove UI error prompting, need to restrucutre the UI layer.
* Selection support unity callback.
* Random utility support include/cover funcation call.
* JCS_FadeObject bug fixed with fade effect behind.
* JCS_FadeObject with better callback function format/naming.
* Update JCS_ButtonSelection with callback.
* Instead of one effect replace with multiple effects for JCS_ButtonSelection.
* Sprite timer conflict with unity object time issue bug fixed.
* Add JCS_GamePadButton base button class for Gamepad interaction.
* JCS_Input with more functional key input naming and readability.
* Now the framework handle multiple controller input with easy function call.
* Add Gam Pad key down and key up check.
* JCS_ButtonSelectionGroup implemented.
* JCS_ButtonSelection implemented.
* JCS_ButtonSelectionGroupController implemented.
* JCS_InputController implemented with default input manager settings.

## 1.5.3
> Released Oct 6, 2017

* Sprite Timer compatible with unity object.
* Add 'NONE' as a default gamepad keycode in JCS_Input script.
* Add handy function enum size in utility layer.
* Add basic two-sided surface shader with transparency.
* Add test key to JCS_FadeObject.
* JCS_FadeObject initial visible boolean bug fixed.
* Audio listener null check bug fixed.
* Add IsVisible variable for base dialogue object.
* Add dialogue object for active and deactive panel button.
* Add zoom function for 2D camera.
* Add Sprite Timer with callback when time is up.
* JCS_TransformTweener update with target and self track.
* Add local and global switch for JCS_TransformTweener.
* Rotate action add with local track.
* JCS_RevolutioAction implemented.
* Include 'UI-Polygon' library made by Credit CiaccoDavide.
* Add circle position function for x, y, z axis.
* Add radian and degree conversion.
* Resolution fix with logo scene.
* Create tween panel added into the JCSUnity editor window.

## 1.5.1
> Released Sep 10, 2017

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
* Fix wrong naming/typo.
* PacketLostPreventer add the timer in order not to send the packet to often and
save a bit of performance comparing 60 packets (frame rate) per second.
* JCS_LookAtMouseAction implemented.
* JCS_ActivePanelButton implemented.
* JCS_DeactivePanelButton implemented.
* JCS_SwitchServerButton implemented.
* JCS_SwitchServerButton implemented.
* JCS_ApplicationCloseSimulateSceneOnline scene added, for online exit client.
* JCS_OpenURLButton implemented.
* Rename PacketDecoder to JCS_DefaultPacketDecoder private same file name.
* Rename PacketEncoder to JCS_DefaultPacketEncoder prevent same file name.
* Out of range Bugs fixed with 'JCS_ServerRequestProcessor'.
* JCS_PacketLostPreventer implemented for tracking down and solve the packet lost
issue by using the UDP for network communicattion.
* Add Asynchronous Datagram Socket for option in 'JCS_NetworkSettings'.
* JCS_UDPGameSocket implemented.
* JCS_Canvas have ability to let other component add to the  resizable panel.
* Add asynchronous callback to socket.
* JCS_ClientMode implemented.
* JCS_RecvPacketType implemented.
* JCS_SendPacketType implemented.
* Add more meaningful to JCS_DefaultClientHandler.
* JCS_PacketProcessor implemented for sample the network handle.
* Error handling for server packet/request.
* JCS_BinaryReader implemented for another framework 'JCSNetS'.
* JCS_Client implemented.
* JCS_ServerRequestProcessor implemented.
* Add require component to 'JCS_NetworkSettings' for 'JCS_ServerRequestProcessor'.
* JCS_PacketHandler implemented.
* JCS_DefaultClientHandler for demo use.
* JCS_Packet for standard byte array storage class.
* Default buffer input and output mem does not set correctlly, settings fixed.
* Client handler logic error in Game Socekt.
* Client handler implemented, so we could have our own packet/buffer handler.

## 1.5.0
> Released Aug 20, 2017

* Update JCS_VelocityInfo with 3 dimentional.
* Re-structure the networking module.
* JCS_Debug no longer need the input of the file.
* JCS_Logger implmeneted for Network module.
* Start the side project JCS_UCG.
* JCS_CameraRenderer implemented.
* GrayScale shader implemented.
* Displacement shader implemented.
* Fixed Item will go through the wall.
* JCS_2DAnimDisplayHolder implemented.
* JCS_ItemWall implemented.
* Fixed JCS_TransformTweener continue tween issue.
* Add Hotkeys category for JCSUnity GUI.
* JCS_ButtonPointer implemented.
* Update Unity version to 5.6.1f1.
* Rename JCS_Cursor to JCS_3DCursor.
* JCS_2DCursor implemented.
* Add slope to the JCS_PositionPlatform.
* JCS_DamageText performance improvement.
* JCS_ClimbableManager implemented.
* JCS_IndieManager implemented.
* Rename JCS_2DRandAnimController to JCS_2DRandAnimByTimeController.
* JCS_2DRandAnimByAnimDone implemented.

## 1.4.5
> Released May 14, 2017

* JCS_FreezeTransformAction implemented.
* JCS_FreezePositionAction implemented.
* JCS_FreezeRotationAction implemented.
* JCS_FreezeScaleAction implemented.
* JCS_RelativeFreezePositionAction implemented.
* JCS_2DOrderAnimPlayer implemented.
* JCS_2DRandAnimController implemented.
* JCS_AdjustTimeTrigger implemented.
* JCS_2DAnimMirror implemented.
* JCS_2DAnimSequencePlayer implemented.
* JCS_2DLadder and JCS_2DRope update with sprite control front and behind.
* JCS_OrderLayer reasonable variable and renamed.
* JCS_OrderLayerObject add multiple SpriteRenderer component as option.
* JCS_DeltaNumber with clear up zero on the left function implemented.
* Rename JCS_SpriteScore to JCS_DeltaNumber.
* JCS_PlatformSettings implemented.
* JCS_RouteGuageSystem implemented.

## 1.4.3
> Released Apr 25, 2017

* JCS_AdvertismentManager implemented.
* JCS_RewardAdsButton implemented.
* Bug fixed JCS_OrderLayer parallax effect jittering issue.
* Zero out the velocity when the camera stopped, bug fixed.
* Update JCS_SoundManager with play one shot of bgm.
* Update JCS_SceneManager with more proper name variables.
* Update JCS_SoundManaer with 'SwitchBackgroundMusic' function.
* Rename JCS_2DDestroyAnimEffect to JCS_3DDestroyAnimEffect.
* JCS_2DDestroyAnimEffect impelemented.
* JCS_TransformPool implemented.
* JCS_DestroySpawnEffect implemented.

## 1.4.1
> Released Apr 4, 2017

* JCS_INIFileReader implemented.
* editor.properties implemented.
* JCS_2DParticleSystem rename to JCS_ParticleSystem.
* JCS_DestroyAnimEffect rename to JCS_2DDestroyAnimEffect.
* Update JCS_ParticleSystem so you could play one shot of particle in certain number of particle.
* JCS_DestroyParticleEndEvent implemented.
* JCS_DestroyParticleEffect implemented.
* JCS_Tweener rename to JCS_TransfromTweener.
* JCS_ColorTweener implemented.

## 1.3.9
> Released Jan 1, 2017

* JCS_3DDistanceTileAction implemented.
* JCS_2DGoStraightAction rename to JCS_3DGoStraightAction.
* JCS_TimePanel implemented.
* JCS_2DAnimation implemented.
* Change the file name old JCS_2DAnimator to JCS_I2DAnimator.
* JCS_2DAnimator implemnted.
* JCS_SpriteScore implemented.
* JCS_HitListEvent update with self-destroy.
* JCS_SpriteTimer implemented.
* JCS_Utility implement DestroyAllTypeObjectInScene function.
* JCS_BasicInitSpawner implemented.
* Redefined JCS_DestroyAnimEffect.
* JCS_BasicWaveSpawner implemented.
* Update JCS_VideoPlayer compatible with Andriod.
* Set JCS_2DCamera virtual function as default.
* JCS_PauseAction implemented.
* JCS_PauseManager implemented.
* Modefied JCS_GameManager's GAME_PAUSE compatible to previous version of JCSUnity.
* Fit Screen Size some bugs fixed.
* JCS_ShowDialogueButton implemented.
* JCS_HideDialogueButton implemented.
* JCS_PauseGameButton implemented.
* JCS_UnPauseGameButton implemented.
* Organize JCSUnity_EditorWindow class.
* Add JCSUnity_EditorWindow for gui interface implementation.
* Add menu item serialize to JCSUnity 2d.
* Add menu item serialize to JCSUnity 3d.
* Add menu item About.
* Add randomize duration at start time for tweener.
* Now All damage text pool provide the specific audio clip function call option.
Meaning all the skill you are designing can be use in different sound.
* For coding wise, after the "ApplyDamageText" function had the new variable
 "AudioClip", pass it in to take the effect.
* Update JCS_2DParticleSytem with "start" and "stop" function
* JCS_InvincibleTimeAction implemented.
* Add scene portal layer.
* Setup the indie folder for managers and settings folder.
* JCS_PortalManager implemented.
* JCS_PortalSetting implemented.
* Add black slide screen while switch scene.
* Setup the shader folder.
* "VertexLit with Z" added.
* FOW shader added.
* JCS_GUIComponentLayer implemented.
* JCS_ItemDroppable update with more function.
* JCS_TowardTarget implemented.
* JCS_DisableWithCertainRangeEvent implemented.
* JCS_PushThrowAction implemented.
* OnLevelWasLoaded deprecated fixed so now version compatible with Unity 5.4 or higher.
* JCS_SceneSettings implemented.
    ■ Fade in/out time for specific scene option.
* JCS_SceneManager effect and update cuz of the JCS_SceneSetting script.
* JCS_3DLookAction asymptotic rotate.
* JCS_3DCamera smooth track function implemented.
* JCS_GUILiquidBar support depend on the alignment.
* JCS_3DLiquidBar support depend on the alignment.
* Joystick implemented.
* JCS_InputSetting updated.
* JCS_3DCamera zoom in/out feature implemented.
* JCS_FadeObject min/max alpha range implemented.
* Project warnings clean up.
* JCS_3DAnimator working on.
* JCS_3DBlendTreeAnimator working on.
* JCS_Tweener updated.
* Update item droppable.
* Multiple script comment.
* Update Pathfinding Request, but there are still some bugs.
* Modefied the name of JCS_GameErros into JCS_Debug, so there is no JCS_GameErros
anymore. Plz use JCS_Debug instead.
* Export JCSUnity 1.3.7 package.
* JCS_2DAIStateSystem implemented.
* Update project to version 5.4.0f3.
* Few bugs fixed compatiable to version 5.4.0f3.
    1) box colliders does not support negative scale
* JCS_VideoPlayer implemented but only support -list file.
    -list
        .mov
* Application end scene implemented.
* A star pathfinding implemented.
* README.txt file updated.
* Working on JCS_VideoPlayer.
* Add image into dialogue system.
* Add name tag into dialogue system.
* Dialogue System implemented.
* Dialogue Script system implemented.
* Workd on A* Pathfinding algorithm.
* README.txt file updated.
* Working on Dialogue Scripting system.
* JCS_ScriptTester scene added.
* UtilitiesScene category added in scene folder.
* npc2100.cs test script updated.
* Update damage text system, so now enemy and player can have different damage text set.
* README.txt file updated.
* In Game Log System implemented.
* JCS_Settings interface implemented.
* Log system implemented into Boss Fight Game.
* JCS_Settings implemented.
* JCS_Managers implemented.
* Damage Text stackover flow logic bug fixed.
* XML Save and Load implemented.
* Get back recover value.
* Health target cannot be damage while the game is over.
* BF_CashText implemented.
* Limit the monster in the scene so it won't spawn too many monster.
* JCS_2DBullet huge updated.
* Character animation fixed.
* Sequence Shoot in Cursor.
* Push Skill implemented.
* Multiple attack implemented.
* JCS_3DMouseMovement implemented.
* BF_HealthTarget implemented.
* JCS_LiquidBarInfo implemented.
* IAP System using Soomla open source project.
* randomize the cash value.
* Item effect object implemented.
* 目前最強的射擊武器.
* JCS_2DInitLookByTypeAction implemented.
* JCS_LiveObjectManager, JCS_2DLiveObjectManager implemented.
* 全圖殺怪.
* JCS_3DLiquidBar implemented.
* JCS_HueController implemented.
* Update Level Text.
* Update inspector controll within the GUI function.
    - JCS_SlideEffect
* JCS_2DCamera smooth scroll implemeneted.
* Mana System game machnice implemented.
* JCS_LiquidBarText implemented.
* use JCS_LiquidBarText into game.
* 打幣系統.
* End Game System implemented.
* BF_RainFaller impelemented.
* JCS_Reflect object more functional with position offset.
* Collision Fixed.
* Cash System implemented.
* Bug Fixed.
* BF_AutoAttacker implemented.
* Boss Fight game scene updated.
* Fixed the collision overlapping by using FixedUpdate() function call from Unity Engine.
* JCS_2DFlyActionIgnore.
* Fly Object ignore all platform in the scene now.
* Arrow Rain implemented.
* AOE function implemented.
* JCS_2DReflectBulletAction.
* JCS_ButtonSoundEffect will have refuse sound when is not interactable.
* JCS_Button interactable function updated.
* JCS_2dFlyAction's basic function implemneted.
* Player attack will wait all star hit the object before die.
* Boss Fight game scene updated.
* Memory Leak bug fixed.
* Wave handler implmented.
* Game Level up implemented.
* JCS_2DDeadAction implmented.
* Update Log Viewer Packages.
* JCS_3DDragDropObject implemented.
* Stars can shoot down each other.
* JCSUnity System updated.
* Player will attack according to the mouse click.
* Live Object animation functional.
* Boss Fight wave handler implemented but not finish yet.
* JCS_GUIStarterScene implemented.
* JCS_2DWalkAction add the random walk speed function at initilzie time.
* JCS_2DJumpAction add the random jump force function at initilzie time.
* JCS_GameErros function update, so no need to enter type the name out anymore,
 but the line number isnt update yet.
* BF_CharacterSpawnHandler implemented, now we can design our own character spawn positions.
* Mad effect for JCS_WalkAction implemented.
* Start the design of the BF.
* LiveObject can now attack player/each other.
* KnowBack algorithm implemented.
* Abiltiy Fromat provide attack and defense value.
* JCS_2DWalkAction bug fixed.
* JCS_VelcotiyInfo more function implemented. MoveSpeed & RecordSpeed.
* JCS_LiveObject more function implemented.
* JCS_2DCursorShootAction implemented.
* JCS_3DCursorShootAction implemented.
* JCS_2DJumpAction implemented.
* JCS_2DWalkAction implemented.
* JCS_VelocityInfo implemented.
* JCS_2DLadder complete!
* JCS_2DLadder bug fixed.
* Player improvement.
* JCS_Physics upgrade.
* JCSUnity Bugs fixed.
* JCS_2DLadder big upgrade.
* JCS_Physics more colliding functions implemented.
* JCS_2DSideScrolldPlayer more functions implemented.
* JCS_3DCamera updated.
* JCS_3DDemo scene make changes.
* Complete Running Crush core system.
* Optimized JCSUnity a bit.
* Start design the new game Boss Fight.
* Select a level before taking image from webcam.
* JCS_RollButtonSelector functional.
* Liquid Bar for Running Crush functional.
* JCS_TweenerPanel new added.
* Liquid Bar for Running Crush.
* SpriteMash liberaris implemented.
* Update JCS_ButtonSoundEffect, now we does not need to add to trigger event manully.
* JCS_EnvironmentSoundPlayer added.
* JCS_Lightning effect.
* JCS_StaticLightning effect.
* Update Detection area to multiple collider.
* JCS_Detect Area Action logic errors fixed.
* JCS_SlideScreenButton can now going direction with counts.
* JCS_SwitchSceneButton can now load scene limited base on platform type.
* RC_EffectItem, so the object will be effect by picking up the item!
* JCS_RollBtnSelector implemented.
* JCS_SequenceSlidePanel implemented.
* JCS_SlideEffect updated.
* JCS_RollBtnSelector implemented.
* JCS_RollSelectorButton implemented.
* JCS_SimpleTrackAction implemented.
* System Updated.
* JCSUnity's GUI system bug fixed.
* Shop System small update.
* Lib updated.
* JCS_SortingObject implemented.
* Shop and Lobby Scene small update.
* Game System update.
* Gold Item.
* Save Load Bug fixed.
* Shop Scene.
* RC_Player can pick up, so player can gain gold.
* Player Pointer.
* Revive Pointer.
* Save Load Function implemented.
* Modefied Tween to general.
* JCS_Item updated.
* Tweener updated with position, rotation, scale.
* RC_EffectObject implemented.
* RC_Goal implemented.
* Game System, so now the the game will act with what mode player chose.
* Save / Load Data implemented.
* Tweener Transition implemented. (Using 3rd party library.)
    - http://gizma.com/easing/#quint1
    - https://github.com/PeterVuorela/Tweener
* JCS_ScaleEffect implemented.
* JCS_ButtonSoundEffect implemented. (use this for any button, working with Unity's Event Trigger script.)
* JCS_SlideScreenButton implemented.
* Work on Example Project.
    - Lobby
    - Webcam
* Part of MO implemented into Running Crush. (Webcam part)
* JCS_Cursor implemented. (Not complete yet.)
* Work on Example Project.
    - Logo
    - Lobby
    - Webcam
    - Game
    - Shop
* Cross Unity Object's Webcam. (Not sure is cross platform yet.)
* Work on Example Project.
* Perspective Camera detect weather the collider area render on the screen or not. (using Game Depth)
* Full Screen Atk implemented.
* Player had the better movement.
* After Jump Effect.
* Particle System
* Weather Particle Interface
* Snow Flake (Weather Particle)
* Rain Drop (Weather Particle)
* Fix Player Settings.
* Swing Attack Action
* Absorb Effect (JCS_ShootAction)
* Sequence Shoot in order right now. (Logic Error fixed.)
* Speed Layer.
* Ignore Item Tag
* Miss Damage Text Code.
* Sprite Auto Facing
* Ability Format working.
* Shoot in sequence
* Bug Fix
* Auto Detect and attack.
* Deviation Throw
* Layer Friction Implemented.
* Abitilty Format interface.
* Hit and Damage Text(JCS_ApplyDamageTextAction)
* Throw Action! (support for 3D right now but not tested yet.)
* Detect Area implemented!
* Drop Item (only with sprite renderer layer.)
* GUI Button Slide.
* Pick up Item action
* Throw Action! (2D only for right now.)
* Update "JCS_PlayerManager".
* Liquid Container(only on X axis!)
* Season implemented.
* 2D&3D Tracking Action Implemented!
* Hit Event implemented!
* Created new category "Event"
* Shake Effect implemented!
* Cos-sine wave effect implemented!
* Mix Damage Text Pool implemented!
* JCS_3DCamera implemented!
* Damage Text Pool implemented!
* Damage Text implemented!
* Checkable feature implemented (when have ur mouse over a checkable
    object it will appear a small description menu)
* Clean Code (there was duplicate code)
* Resolution for all platform feature added (Tested! and it work!)
* Slide Input implemented (for touch pad)
* Application Manager managing the platform type form now on!
* Slide Switch Scene Function Completed
* Resolution for all platform feature added (Not Tested!)
* Bug Fix ("JCS_GameWindow" script)
* Bug Fix (Logog Scene)
* Multi Track Camera 2D feature add
* Pair Key Panel and Button Panel up
* Scene Manager bug fixed
* Multi Track Camera 2D
* Sound Fade in/out while switching the scene
* Bug Fix (Logo Scene)
* Top Down Player added
