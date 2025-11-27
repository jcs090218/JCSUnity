# JCS_Util

All code utility is stored here.

## Functions

| Name                                 | Description                                                                                       |
|:-------------------------------------|:--------------------------------------------------------------------------------------------------|
| Delta                                | Delta the `num` with `val` and clamp the result with `min` and `max`.                             |
| DeltaP                               | Delta the `num` with `val` by percentage and clamp the result with `min` and `max`.               |
| BytesToString                        | Convert byte array to string by charset type.                                                     |
| StringToBytes                        | Convert string to byte array by charset type.                                                     |
| EscapeURL                            | Simple version of escape url.                                                                     |
| WithInRange                          | Check if the value is within the range.                                                           |
| WithInRange                          | Check if the index is valid within the collection's size.                                         |
| CopyBtyeArray                        | Copy byte array to another byte array memory space.                                               |
| IsNumberString                       | Check if the string is a number string.                                                           |
| Parse                                | Parse `str` to integer, return `defaultValue` if failed.                                          |
| Parse                                | Parse `str` to float, return `defaultValue` if failed.                                            |
| Parse                                | Parse `str` to boolean, return `defaultValue` if failed.                                          |
| ToJson                               | Serialize object to JSON string.                                                                  |
| GetComponentAbove                    | Get the component from current and above layers.                                                  |
| GetComponentBelow                    | Get the component from current and below layers.                                                  |
| GetComponentAll                      | Get the component from all layers.                                                                |
| GetComponentsAbove                   | Get components from current and above layers.                                                     |
| GetComponentsBelow                   | Get components from current and below layers.                                                     |
| GetComponentsAll                     | Get components from all layers.                                                                   |
| EnableComponent                      | Do enable/distance component.                                                                     |
| EnableComponents                     | Set enable/disable to all component on this transform.                                            |
| SetActiveToAllChildren               | Active all the child in a transform.                                                              |
| MoveToTheLastChild                   | Make the transform to the last transform of the current parent transform.                         |
| SetParentWithoutLosingInfo           | Set the transform to another transform without losing it's info. (position, rotation, scale)      |
| DetachChildren                       | Detach all the child from a transform.                                                            |
| ForceDetachChildren                  | Force detach all the child from a transform.                                                      |
| AttachChildren                       | Attach all the childs to this transform.                                                          |
| Instantiate                          | Spawn a game object.                                                                              |
| InstantiateToScene                   | Spwan a game object to another scene.                                                             |
| WithActiveScene                      | Execute within the active scene without losing the current scene.                                 |
| SpawnAnimateObject                   | Spawn a game object with animation attached.                                                      |
| SpawnAnimateObjectDeathEvent         | Spawn a game object with the animator and death event on it.                                      |
| DestroyAllTypeObjectInScene          | Destory all game objects in the scene with the type passed in.                                    |
| DestroyImmediateAllTypeObjectInScene | Destroy all the game object in the scene immediately with the type passed in.                     |
| IsClone                              | Return true if the object is a clone.                                                             |
| RemoveCloneString                    | Remove the text "(Clone)" from the object's name, and return the new name string.                 |
| FindObjectByType                     | Retrieves the first active loaded object of Type type.                                            |
| FindObjectsByType                    | Retrieves a list of all loaded objects of Type type.                                              |
| FindGameObjectsWithTag               | Return a list of game object with the tag name.                                                   |
| FindCloneObjectsOfTypeAll            | Find all cloned game objects in the scene with the type passed in.                                |
| FindNotCloneObjectsOfTypeAll         | Find all game objects that are not clones in the scene with the type passed in.                   |
| FindObjectsOfTypeAllInHierarchy      | Find all the game object that are only active in the hierarchy with the type passed in.           |
| GetEasing                            | Returns the easing function pointer base on the tweener type/enum.                                |
| IsScene                              | Check current scene's with NAME.                                                                  |
| IsSceneExists                        | Returns true if the scene 'name' exists and is in your Build settings, false otherwise.           |
| PlayParticleAtPoint                  | Play the particle at point.                                                                       |
| DestroyParticle                      | Destroy the particle by its duration.                                                             |
| IsSameTribe                          | Check if the live object is the same tribe.                                                       |
