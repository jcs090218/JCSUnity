# JCS_Item

Base class for all the item subclasses.

## Variables

| Name                                      | Description                                                |
|:------------------------------------------|:-----------------------------------------------------------|
| onPicked                                  | Execution after the item is picked.                        |
| mMustBeActivePlayer                       | Must be an active player in order to pick this item up.    |
| mPickKey                                  | Key to pick this item up.                                  |
| mPickByMouseDown                          | Pick up the item by mouse.                                 |
| mAutoPickColliderTouched                  | When player hit this object pick it up automatically.      |
| mAutoPickWhileCan                         | Automatically pick it up while this item can be picked up. |
| mPlayOneShotWhileNotPlayingForPickSound   | Play one shot while not playing any other sound.           |
| mPickSound                                | Sound played when you pick up this item.                   |
| mPlayOneShotWhileNotPlayingForEffectSound | Play one shot while not playing any other sound.           |
| mEffectSound                              | Sound played when you pick up this item.                   |
| mTweener                                  | Make item tween to the destination.                        |
| mDestinationDestroy                       | Destroy when reach the destination.                        |

## Functions

| Name | Description       |
|:-----|:------------------|
| Pick | Pick the item up. |
