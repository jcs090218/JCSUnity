# JCS_ApplicationManager

Interface communicate to application layer.

## Variables

| Name                   | Description                       |
|:-----------------------|:----------------------------------|
| SIMULATE_PLATFORM_TYPE | Target platform type to simulate. |
| PLATFORM_TYPE          | This will override Platform Type. |

## Functions

| Name             | Description                                           |
|:-----------------|:------------------------------------------------------|
| isPC             | Return true if current platform is personal computer. |
| isMobile         | Return true if current platform is mobile phone.      |
| Quit             | Quit the application.                                 |
| AddLangText      | Register a new language text.                         |
| RefreshLangTexts | Refresh all languages text in game.                   |
