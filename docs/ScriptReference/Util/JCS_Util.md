# JCS_Util

All code utility is stored here.

## Functions

| Name                                 | Description                                                                                       |
|:-------------------------------------|:--------------------------------------------------------------------------------------------------|
| EnableComponent                      | Do enable/distance component.                                                                     |
| SpawnGameObject                      | Spawn a game object.                                                                              |
| FindObjectByType                     | Retrieves the first active loaded object of Type type.                                            |
| FindObjectsByType                    | Retrieves a list of all loaded objects of Type type.                                              |
| WithInRange                          | Check if the value is within the range.                                                           |
| WithInArrayRange                     | Check if the index valid within the array length.&nbsp&nbsp(0 ~ (length - 1))                     |
| LoopInArray                          | Make the index is within the array length by setting the maxinum of (legnth - 1) or mininum of 0. |
| GetImageRect                         | Returns the size of the image.                                                                    |
| GetSpriteRendererRectWithNoScale     | Returns the size of the sprite renderer without the scale value multiply.                         |
| GetSpriteRendererRect                | Returns the size of the sprite renderer.                                                          |
| MouseOverGUI                         | Check if the mouse is on top of a recttransform.                                                  |
| IsOverGUI                            | Check if mosue is over any UI element.                                                            |
| SpawnAnimateObject                   | Spawn a gameobject with animation attached.                                                       |
| SpawnAnimateObjectDeathEvent         | Spawn a gameobject with the animator and death event on it.                                       |
| SetActiveToAllChildren               | Active all the child in a transform.                                                              |
| MoveToTheLastChild                   | Make the transform to the last transform of the current parent transform.                         |
| SetParentWithoutLosingInfo           | Set the transform to another transform without losing it's info. (position, rotation, scale)      |
| AddEventTriggerEvent                 | Add an event to EventTrigger component from Unity API.                                            |
| VectorDirection                      | Returns vector direction by passing the vector direction enum.                                    |
| ApplyRandVector3                     | Add random value to a vector.                                                                     |
| IsSameTribe                          | Check if the live object is the same tribe.                                                       |
| GetValues                            | Get the value for each enum, use to loop through the enum.                                        |
| SetEnableAllComponents               | Set enable/disable to all component on this transform.                                            |
| DestroyAllTypeObjectInScene          | Destory all gameobjects in the scene with the type passed in.                                     |
| DestroyImmediateAllTypeObjectInScene | Destroy all the gameobject in the scene immediately with the type passed in.                      |
| FindCloneObjectsOfTypeAll            | Find all cloned gameobjects in the scene with the type passed in.                                 |
| FindNotCloneObjectsOfTypeAll         | Find all gameobjects that are not clones in the scene with the type passed in.                    |
| FindObjectsOfTypeAllInHierarchy      | Find all the gameobject that are only active in the hierarchy with the type passed in.            |
| GetEasing                            | Returns the easing function pointer base on the tweener type/enum.                                |
| FillSlot                             | Fill slots with initialize value type by length.                                                  |
| RemoveEmptySlot                      | Remove the null value from a list/array.                                                          |
| RemoveEmptySlotIncludeMissing        | Remove all the null value including missing reference in the list/array.                          |
| GetFinalLocalEulerAngles             | Get the final local euler angles by accumilate all the parent gameobjects.                        |
| MergeArrays                          | Merge multiple arrays into one array.                                                             |
| MergeArrays2                         | Merge two array and return the new array.                                                         |
| MergeList                            | Merge the two lists and return the new list.                                                      |
| CopyBtyeArray                        | Copy byte array to another byte array memory space.                                               |
| EnumSize                             | Returns the length of an enumerator.                                                              |
| IsArrayEmpty                         | Check if the string array is empty.                                                               |
| ListPopFront                         | Pop the first value from the list.                                                                |
| ListPopBack                          | Pop the last value from the list.                                                                 |
| IsNumberString                       | Check if the string is a number string.                                                           |
| DetachChildren                       | Detach all the child from a transform.                                                            |
| ForceDetachChildren                  | Force detach all the child from a transform.                                                      |
| AttachChildren                       | Attach all the childs to this transform.                                                          |
| ForceGetComponent                    | Force to get a compoennt, if not found add one new then.                                          |
| ToJson                               | Serialize object to JSON string.                                                                  |
| IsScene                              | Check current scene's with NAME.                                                                  |
| LastFileIndex                        | Method to do search directory and get the last file index.                                        |
| DeleteAllFilesFromDir                | Delete all files in directory.                                                                    |
| BytesToString                        | Convert byte array to string by charset type.                                                     |
| StringToBytes                        | Convert string to byte array by charset type.                                                     |
| EscapeURL                            | Simple version of escape url.                                                                     |
