# JCS_SpriteTimer

Timer system using sprite 0 to 9.

## Variables

| Name               | Description                            |
|:-------------------|:---------------------------------------|
| mActive            | Timer active or not active.            |
| mCurrentHours      | Current hours in the game.             |
| mCurrentMinutes    | Current minutes in the game.           |
| mCurrentSeconds    | Current seconds in the game.           |
| mRoundUp           | Do round up instead of round down.     |
| mTimeType          | Type of the delta time.                |
| mTimeText0         | Time text sprite 0.                    |
| mTimeText1         | Time text sprite 1.                    |
| mTimeText2         | Time text sprite 2.                    |
| mTimeText3         | Time text sprite 3.                    |
| mTimeText4         | Time text sprite 4.                    |
| mTimeText5         | Time text sprite 5.                    |
| mTimeText6         | Time text sprite 6.                    |
| mTimeText7         | Time text sprite 7.                    |
| mTimeText8         | Time text sprite 8.                    |
| mTimeText9         | Time text sprite 9.                    |
| mDigitHour1        | Each digit for hour.                   |
| mDigitHour2        | Each digit for hour.                   |
| mDigitMinute1      | Each digit for minute.                 |
| mDigitMinute2      | Each digit for minute.                 |
| mDigitSecond1      | Each digit for second.                 |
| mDigitSecond2      | Each digit for second.                 |
| mDigitInterval     | Interval between each digit.           |
| mDigitUnitInterval | Interval between each unit digit       |
| mHourSound         | Sound played when hours get reduced.   |
| mMinuteSound       | Sound played when minutes get reduced. |
| mSecondSound       | Sound played when seconds get reduced. |

## Functions

| Name                 | Description                                        |
|:---------------------|:---------------------------------------------------|
| SetCurrentTime       | Set the current time.                              |
| IsTimeUp             | Check if the time is up?                           |
| UpdateTimeInterval   | Update the Time GUI base on this particular order. |
| UpdateHourInterval   | Update hour interval.                              |
| UpdateMinuteInterval | Update minute interval.                            |
| UpdateSecondInterval | Update second interval.                            |
| UpdateTimeUI         | Update the time UI.                                |
