# JCS_DeltaNumber

System that control 0-9 number sprites and create a counter effect.

## Variables

| Name                 | Description                                  |
|:---------------------|:---------------------------------------------|
| mCurrentScore        | Current score rendering.                     |
| mScoreNull           | <Default null slot.                          |
| mScoreText0          | Score text 0.                                |
| mScoreText1          | Score text 1.                                |
| mScoreText2          | Score text 2.                                |
| mScoreText3          | Score text 3.                                |
| mScoreText4          | Score text 4.                                |
| mScoreText5          | Score text 5.                                |
| mScoreText6          | Score text 6.                                |
| mScoreText7          | Score text 7.                                |
| mScoreText           | Score text 8.                                |
| mScoreText9          | Score text 9.                                |
| mDigitsRendererSlot  | Each digit, the more length the more digit.  |
| mDigitInterval       | Interval between each digit.                 |
| mClearEmptyLeftZero  | Clear all the empty zero from the left.      |
| mVisibleOnZero       | Is visible when is zero?                     |
| mTextAlign           | Align side.                                  |
| mMaxScore            | Maxinum score.                               |
| mMinScore            | Mininum score.                               |
| mDeltaToCurrentScore | Show each digit between the score animation. |
| mTargetScore         | Current targeting score.                     |
| mAnimScoreTime       | How fast the score animate.                  |
| mDeltaProduct        | How much the delta value add up.             |

## Functions

| Name                       | Description                               |
|:---------------------------|:------------------------------------------|
| EnableDigitsRendererSlot   | Set enable/disable all digit render slot. |
| UpdateScore                | Update the score GUI.                     |
| UpdateIntervalForEachDigit | Update the space to each digit.           |
