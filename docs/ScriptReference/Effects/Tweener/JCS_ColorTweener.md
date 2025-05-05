# JCS_ColorTweener

Color tweener.

## Variables

| Name           | Description                              |
|:---------------|:-----------------------------------------|
| onStart        | Callback to execute when start tweening. |
| onDone         | Callback to execute when done tweening.  |
| mTimeType      | Type of the delta time.                  |
| mEaseTypeR     | Tween type for red channel.              |
| mEaseTypeG     | Tween type for green channel.            |
| mEaseTypeB     | Tween type for blue channel.             |
| mEaseTypeA     | Tween type for alpha channel.            |
| mDurationRed   | How fast it changes the red channel.     |
| mDurationGreen | How fast it changes the green channel.   |
| mDurationBlue  | How fast it changes the blue channel.    |
| mDurationAlpha | How fast it changes the alpha channel.   |
| mIgnoreR       | Do not do anything with red channel.     |
| mIgnoreG       | Do not do anything with green channel.   |
| mIgnoreB       | Do not do anything with blue channel.    |
| mIgnoreA       | Do not do anything with alpha channel.   |
| mUnityCallback | Callback after easing.                   |

## Functions

| Name         | Description                 |
|:-------------|:----------------------------|
| ResetTweener | Reset the tweener progress. |
| DoTween      | Tween to the color.         |
