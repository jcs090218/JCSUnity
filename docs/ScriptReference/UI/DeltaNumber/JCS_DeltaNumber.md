# JCS_DeltaNumber

System that controls 0-9 number sprites and creates a counter effect.

## Variables

| Name                  | Description                                   |
|:----------------------|:----------------------------------------------|
| mCurrentNumber        | Current number rendering.                     |
| mNumberNull           | <Default null slot.                           |
| mNumberText0          | Number image 0.                               |
| mNumberText1          | Number image 1.                               |
| mNumberText2          | Number image 2.                               |
| mNumberText3          | Number image 3.                               |
| mNumberText4          | Number image 4.                               |
| mNumberText5          | Number image 5.                               |
| mNumberText6          | Number image 6.                               |
| mNumberText7          | Number image 7.                               |
| mNumberText8          | Number image 8.                               |
| mNumberText9          | Number image 9.                               |
| mDigitsRendererSlot   | Each digit, the more length the more digit.   |
| mDigitInterval        | Interval between each digit.                  |
| mClearEmptyLeftZero   | Clear all the empty zero from the left.       |
| mVisibleOnZero        | Is visible when is zero?                      |
| mTextAlign            | Align side.                                   |
| mTimeType             | Type of the delta time.                       |
| mMaxNumber            | Maxinum number.                               |
| mMinNumber            | Mininum number.                               |
| mDeltaToCurrentNumber | Show each digit between the number animation. |
| mTargetNumber         | Current targeting number.                     |
| mAnimNumberTime       | How fast the number animate.                  |
| mDeltaProduct         | How much the delta value add up.              |

## Functions

| Name                       | Description                               |
|:---------------------------|:------------------------------------------|
| EnableDigitsRendererSlot   | Set enable/disable all digit render slot. |
| UpdateNumber               | Update the number GUI.                     |
| UpdateIntervalForEachDigit | Update the space to each digit.           |
