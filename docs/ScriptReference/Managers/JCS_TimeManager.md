# JCS_TimeManager

Holding all the information about the time in real life.

## Variables

| Name                | Description                       |
|:--------------------|:----------------------------------|
| mCurrentTimeRange   | Time range at the current region. |
| mCurrentSeasonType  | Season current time.              |
| mCurrentWeatherType | Weather current time period.      |

## Functions

| Name                 | Description                                      |
|:---------------------|:-------------------------------------------------|
| GetCurrentTime       | Get the current time in base on time zone in OS. |
| LoadCurrentTimeRange | Get the current time base on the OS time.        |
| isInTheMorning       | Is current time in the morning. (5am ~ 8am)      |
| isMorning            | Is current time morning. (8pm ~ 12pm)            |
| isNoon               | Is current time noon? (12pm ~ 14pm)              |
| isAfternoon          | Is current time afternoon? (14pm ~ 17pm)         |
| isEvening            | Is current time evening? (17pm ~ 19pm)           |
| isAtNight            | Is current time night? (19pm ~ 22pm)             |
| isLateAtNight        | Is current time late at night? (22pm ~ 2am)      |
| isBeforeDawn         | Is current time before dawn? (2am ~ 5am)         |
| GetCurrentSeason     | Get the season base on the OS's time's month.    |
| isSpring             | Is sping time? (3 ~ 5)                           |
| isSummer             | Is summer time? (6 ~ 8)                          |
| isFall               | Is fall time? (9 ~ 11)                           |
| isWinter             | Is winter time? (12 ~ 2)                         |
| GetCurrentWeather    | Get the current weather base on the website api. |
