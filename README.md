[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Unity Engine](https://img.shields.io/badge/unity-6000.1.0f1-black.svg?style=flat&logo=unity)](https://unity3d.com/get-unity/download/archive)
[![.NET](https://img.shields.io/badge/.NET-4.x-blueviolet.svg)](https://docs.unity3d.com/2018.3/Documentation/Manual/ScriptingRuntimeUpgrade.html)
[![Release](https://img.shields.io/github/tag/jcs090218/JCSUnity.svg?label=release&logo=github)](https://github.com/jcs090218/JCSUnity/releases/latest)

# JCSUnity

[![License](https://github.com/jcs090218/JCSUnity/actions/workflows/license.yml/badge.svg)](https://github.com/jcs090218/JCSUnity/actions/workflows/license.yml)
[![Build](https://github.com/jcs090218/JCSUnity/actions/workflows/build.yml/badge.svg)](https://github.com/jcs090218/JCSUnity/actions/workflows/build.yml)
[![Package](https://github.com/jcs090218/JCSUnity/actions/workflows/package.yml/badge.svg)](https://github.com/jcs090218/JCSUnity/actions/workflows/package.yml)
[![Docs](https://github.com/jcs090218/JCSUnity/actions/workflows/docs.yml/badge.svg)](https://github.com/jcs090218/JCSUnity/actions/workflows/docs.yml)

JCSUnity is a component driven framework built to allow the user
to quickly generate commonly used game mechanics and features
such as UI functionality and player movement. This will allow
for a faster initial development. Most components work individually
while other components must be used in tandem with others.
Individually components can be combined to create new behaviors.

## 💥 Important Notice

All media assets inside JCSUnity do not fall under its license.
These assets are placeholders only, and should only be used for
testing purposes. They are NOT, under any circumstances, to be
used commercially. By using this framework you agree to these
terms, and understand that you are solely responsible for any
legal action taken against you for using, or attempting to use
any placeholder assets commercially.

## 📰 News

Here is the list of few important and recent changes to this framework.

- `3.1.0` - Revamp the framework to reflect current best practices.
- `3.0.0` - Rename setting function from `CheckSingleton` to `CheckInstance`.
- `2.3.1` - Use [MyBox](https://github.com/Deadcows/MyBox) to organize variables.
- `2.3.0` - Fixed resizable screen's calculation in perspective mode.
- `2.2.1` - Implement new screen type, `MIXED` for responsive UI.
- `2.2.0` - Add `trimmed` version to release.
- `2.1.2` - Add support for multiple languages.
- `2.1.1` - Add support for safe area view for iPhone X or above.
- `2.1.0` - Multiple minor fix for screen module for resizable screen feature.
- `2.0.7` - Support consistent streaming assets loading with web request.

## 🔨 How to use it?

JCSUnity is like other Unity plugins. You can download the latest release
from the above tab, or from the link
[here](https://github.com/jcs090218/JCSUnity/releases/latest)
. Simply create a new project in Unity then import all of the assets into
that project. Then you can start all of the tools in the JCSUnity framework.

## 🏆 Features

So, what does this framework does? Check out the
[features](https://github.com/jcs090218/JCSUnity/tree/master/features)
directory at the root of the project directory.

## 📌 Dependencies

These are libraries `JCSUnity` uses. Consider the usage, some plugins aren't necessary
depend on the game your are making.

- [Newtonsoft Json](https://www.newtonsoft.com/json) (required) - Popular high-performance JSON framework for .NET.
  - Install with url [`com.unity.nuget.newtonsoft-json`](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html); see [Newtonsoft.Json-for-Unity/wiki](https://github.com/applejag/Newtonsoft.Json-for-Unity/wiki).
- [MyBox](https://github.com/Deadcows/MyBox) (required) - is a set of attributes, tools and extensions for Unity
- [Tweener](https://github.com/PeterVuorela/Tweener) (required) - Simpler and light weight tweener library.
- [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068) (optional) - Easier debugging after built.
- [StandaloneFileBrowser](https://github.com/gkngkc/UnityStandaloneFileBrowser) (optional) - File browser for UI/tool base application.
- [UI-Polygon](https://github.com/CiaccoDavide/Unity-UI-Polygon) (optional) - Geometry shape UI renderer interface.

## 🔗 Links

- *Demo* : https://www.youtube.com/playlist?list=PLp13qyXnE6zDioC30eW_-aqr4gsswxJo0
- *Tutorials* : https://www.youtube.com/playlist?list=PLp13qyXnE6zCmRobHV9uEjv-1_ieCPwjc
- *Manual/Scripting API* : [Scripting-Manual-JCSUnity](https://jcs090218.github.io/JCSUnity/Manual/index.html)

## 🎮 Showcase

Here is a list of applications that are made with JCSUnity.

<a href="https://apkcombo.com/twilight-tower-livevr/com.AAU.TwilightTower/" target="_blank">
  <img src="./etc/games/Twilight_Tower.png" width="8%" align="middle"/>
</a>
<a href="https://apkcombo.com/hemlock-and-the-horrible-net/com.aau.jcs/" target="_blank">
  <img src="./etc/games/Hemlock.png" width="7%" align="middle"/>
</a>
<a href="https://youtu.be/OQqqgbf0mGI" target="_blank">
  <img src="./etc/games/Radiant_Rune_Fist.png" width="18%" align="middle"/>
</a>
<a href="https://www.youtube.com/watch?v=vPapMMxzNGg&feature=youtu.be" target="_blank">
  <img src="./etc/games/Might_&_Blade.png" width="18%" align="middle"/>
</a>
<a href="https://mwgamedesign.itch.io/sugar-sleuths" target="_blank">
  <img src="./etc/games/Sugar_Sleuths.png" width="15%" align="middle"/>
</a>
<a href="http://www.jcs-profile.com/public/links/Links_PipelineOfEmperorYu/" target="_blank">
  <img src="./etc/games/PEY.png" width="7%" align="middle"/>
</a>
<a href="https://meteo.com.tw/app-download.html" target="_blank">
  <img src="./etc/app/Meteo.png" width="7%" align="middle"/>
</a>
<a href="https://apps.apple.com/us/app/lights-delights/id1541283833" target="_blank">
  <img src="./etc/app/LnD.png" width="7%" align="middle"/>
</a>
<a href="https://apps.apple.com/us/app/monumental-conversations/id1585909435" target="_blank">
  <img src="./etc/app/MonCon.png" width="7%" align="middle"/>
</a>
<a href="https://kuhhenry.itch.io/alice-in-surprise" target="_blank">
  <img src="./etc/games/AIS.png" width="7%" align="middle"/>
</a>
<a href="https://jcs090218.itch.io/you-have-an-order" target="_blank">
  <img src="./etc/games/Crypt_Adv.png" width="7%" align="middle"/>
</a>
