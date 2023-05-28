# JCS_3DLiquidBar

3D liquid bar; does like the health bar, mana bar, etc.

## Variables

| Name               | Description                                     |
|:-------------------|:------------------------------------------------|
| mSpriteMask        | Sprite mask that mask out the inner bar sprite. |
| mBarSpriteRenderer | Please put the under texture bar here.          |

## Functions

| Name              | Description                                        |
|:------------------|:---------------------------------------------------|
| AttachInfo        | Update the liquid bar by attach info.              |
| SetMaxValue       | Set the maxinum value of this container.           |
| SetMinValue       | Set the mininum value of this container.           |
| SetCurrentValue   | Set current liquid bar value.                      |
| DeltaCurrentValue | Delta the current value.                           |
| DeltaAdd          | Delta to current value in absolute positive value. |
| DeltaSub          | Delta to current value in absolute negative value. |
| Full              | Full the liquid bar.                               |
| Lack              | Zero the liquid bar.                               |
| IsAbleToCast      | Check if the value are able to cast.               |
| IsAbleToCastCast  | Check if able to cast the spell, if true cast it.  |
