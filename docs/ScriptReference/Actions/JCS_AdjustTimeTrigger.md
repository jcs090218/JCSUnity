# JCS_AdjustTimeTrigger

Trigger an event while the time is reached.

## Variables

| Name           | Description                               |
|:---------------|:------------------------------------------|
| onAction       | Action to trigger if the time is reached. |
| mInvokeOnStart | Run immediately on the first frame.       |
| mActive        | Is this component active?"                |
| mTime          | Time to trigger the event.                |
| mAdjustTime    | Time that will randomly affect the time.  |
| mTimeType      | Type of the delta time.                   |
| mUnityEvents   | Event that will be triggered.             |

## Functions

| Name                         | Description                                     |
|:-----------------------------|:------------------------------------------------|
| ResetAndRun                  | Reset time and timer, then run the action once. |
| RecalculateTimeAndResetTimer | Recalculate the time and reset the timer.       |
| ResetRecalculateTime         | Recalculate the real time.                      |
| ResetTimer                   | Reset the timer.                                |
