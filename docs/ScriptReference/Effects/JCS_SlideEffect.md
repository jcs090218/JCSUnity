# JCS_SlideEffect

Effect that slide the object to one position.

## Variables

| Name                      | Description                                          |
|:--------------------------|:-----------------------------------------------------|
| mIsActive                 | Is this effect currently active.                     |
| mPanelRoot                | The panel root object.                               |
| mEventTrigger             | Event trigger system.                                |
| mAxis                     | Direction object slides.                             |
| mDistance                 | How far the object slides.                           |
| mFriction                 | How fast the object slides.                          |
| mTimeType                 | Type of the delta time.                              |
| mAutoAddEvent             | Add event to event trigger system.                   |
| mActiveEventTriggerType   | Event trigger type to active the the slide effect.   |
| mDeactiveEventTriggerType | Event trigger type to deactive the the slide effect. |
| mActiveClip               | If slide out, do the sound.                          |
| mDeactiveClip             | If slide back the original position, do the sound.   |
| mIgnoreX                  | Don't track on x-axis.                               |
| mIgnoreY                  | Don't track on y-axis.                               |
| mIgnoreZ                  | Don't track on z-axis?                               |
| mActiveButton             | Button that active this effect.                      |

## Functions

| Name            | Description                    |
|:----------------|:-------------------------------|
| JCS_OnMouseOver | Call it when is on mouse over. |
| JCS_OnMouseExit | Call it When is on mouse exit. |
| Active          | Active the effect.             |
| Deactive        | Deactive the effect.           |
| IsIdle          | Is the object idle.            |
