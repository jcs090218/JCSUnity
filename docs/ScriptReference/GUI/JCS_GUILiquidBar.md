# JCS_GUILiquidBar

Any liquid bar like health bar, mana bar, etc.

## Variables

| Name  | Description          |
|:------|:---------------------|
| mMask | Mask for liquid bar. |

## Functions

| Name              | Description                                        |
|:------------------|:---------------------------------------------------|
| SetMaxValue       | Set the maxinum value of this container.           |
| SetMinValue       | Set the mininum value of this container.           |
| SetCurrentValue   | Set the value directly.                            |
| Full              | Full the liquid bar.                               |
| Lack              | Zero out the liquid bar.                           |
| DeltaCurrentValue | Delta the current value.                           |
| DeltaAdd          | Delta to current value in absolute positive value. |
| DeltaSub          | Delta to current value in absolute negative value. |
| IsAbleToCast      | Check if the value are able to cast.               |
| IsAbleToCastCast  | Check if able to cast the spell, if true cast it.  |
| GetCurrentValue   | Returns current value in liquid bar.               |
