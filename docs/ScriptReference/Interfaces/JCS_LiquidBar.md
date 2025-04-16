# JCS_LiquidBar

Liquid bar object's interface declaration.

## Variables

| Name                     | Description                                                     |
|:-------------------------|:----------------------------------------------------------------|
| OverrideZero             | Once it set to zero, but still override this.                   |
| mAlign                   | Align on which side? (top/bottom/right/left)                    |
| mTimeType                | Type of the delta time.                                         |
| DeltaFriction            | How fast the liquid bar move approach to target position/value. |
| MinValue                 | Mininum value of the liquid bar.                                |
| MaxValue                 | Maxinum value of the liquid bar.                                |
| CurrentValue             | Current liquid bar value.                                       |
| Info                     | Liquid bar info that will be use for this liquid bar.           |
| InfoImage                | Information Image set here.                                     |
| RecoverEffect            | Enable the recover effect?                                      |
| TimeToRecover            | Time for one recover.                                           |
| RecoverValue             | Recover Value per time.                                         |
| BackToRecordRecoverValue | Will try to go back to the original value.                      |

## Functions

| Name          | Description                                                     |
|:--------------|:----------------------------------------------------------------|
| SetInfoSprite | Set the source sprite from the Image component in Unity Engine. |
