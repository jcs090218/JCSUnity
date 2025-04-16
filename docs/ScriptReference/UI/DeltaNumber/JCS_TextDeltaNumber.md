# JCS_TextDeltaNumber

Like `JCS_DeltaNumber`, but instead of altering the sprite, we alter the text
instead.

## Variables

| Name                  | Description                                      |
|:----------------------|:-------------------------------------------------|
| mCurrentNumber        | Current number that will turn into string.       |
| mPlusSignWhenPositive | Ensure add a plus sign if the numer is positive. |
| mPreString            | String added before rendering the number."       |
| mPostString           | String added after rendering the number.         |
| mRoundPlace           | Place you want to round the decimal.             |
| mTimeType             | Type of the delta time.                          |
| mMaxNumber            | Maxinum number.                                  |
| mMinNumber            | Mininum number.                                  |
| mDeltaToCurrentNumber | Show each digit between the number animation.    |
| mTargetNumber         | Current targeting number.                        |
| mAnimNumberTime       | How fast the number animate.                     |
| mDeltaProduct         | How much the delta value add up.                 |

## Functions

| Name         | Description                  |
|:-------------|:-----------------------------|
| UpdateNumber | Start the text delta number. |
