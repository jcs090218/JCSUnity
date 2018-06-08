# JCSUnity #

JCSUnity is a component driven framework. There are a tone 
of reusable components that are already pre-built. 
Most component work individuslly, except some of them do not. 
You can combine/attach two components on the same game object 
and it will make completely different behaviour since the first 
component works on it own and so do the second component. Two of the 
components works individually will create the brand new behaviour. 
The best part of the framework is by knowing these components always 
suprise me how much I can do with my creativity and not just 
engineering. There are one thing that this framework do the best is 
fast and being productive building a game.
<br/>

## Important Notice ##
All media assets inside JCSUnity do not fall under its license. 
These assets are placeholders only, and should only be used for 
testing purposes. They are NOT, under any circumstances, to be 
used commercially. By using this framework you agree to these 
terms, and understand that you are solely responsible for any 
legal action taken against you for using, or attempting to use 
any placeholder assets commercially.
<br/>

## How to use it? ##
JCSUnity is like other Unity plugins. You can download the latest 
release from the above tab, or from the link 
<a href="https://github.com/jcs090218/JCSUnity_Framework/releases/latest" target="_blank">
here</a>. Simply create a new project in Unity then import all of 
the assets into that project. Then you can start all of the tools 
in the JCSUnity framework. <br/>

## Current Version Status ##
JCSUnity Version => `1.6.5`
<br/>
Unity Version => `2017.4.2f2 (64-bit)`
<br/>

<br/>

## Overview ##

### Create Scene with JCSUnity ###
Create the simple scene with smooth switching the scene and smooth
switching the background music is always painful. Not because is
hard but is work that you will have to do for mostly every project. 
I made the 'JCSUnity' editor for just one click so you could have
nice switching scene UI and background music. Background music can
be switch at 'JCS_Settings' object to 'JCS_SoundSetting' component 
in the scene. Scene relative variables can be found at 'JCS_Settings' 
object 'JCS_SceneSetting' component.<br/>
<img src="./screen_shot/fast_create_scene.gif"/>

### Auto Resize ###
I use to hate drag and move the anchor point around when dealing
with different resolutions in Unity UI system. Although, this
cost a bit of performance at initialize time, I think is worth it to
have this feature because I will never have to drag the anchor point
around everytime I start a new project or create a new panel in the 
scene. <br/>
<img src="./screen_shot/auto_resize.gif"/>

### GUI System ###
Since Unity version 4.6, they have release nice uGUI system, but 
with lack of cool effect and sound on there. Here are some simple
effect I made so you can simple make game with details. <br/>
<img src="./screen_shot/GUI_system.gif"/>

### Network Module ###
I have never use Unity's network module, but I heard a lot of people
complain how bad Unity handle networking/socket programming. I provide
the basic client side TCP and UDP socket class and some switch 
port/host function, so you can use it with the server side code that 
you confortable with. <br/>
<img src="./screen_shot/network_module.gif"/>

### Dialogue System ###
Do you ever had an issue implementing dialogue in Unity? Here is basic
dialogue system which is easy to customize. You can control the text
scroll speed and all the images' position. Just inherent 'JCS_DialogueScript'
class to design you own dialogue! You can test your script in 'JCS_ScriptTeseter'
scene. <br/>
<img src="./screen_shot/dialogue_system.gif"/>

### In Game Log System ###
Log system inside the game. <br/>
<img src="./screen_shot/IGLog_system.gif"/>

## Demo ##
* https://www.youtube.com/playlist?list=PLZgPIJqrkb83SBfBSzk0SMchegZFO9lKI

## Tutorials ##
* https://www.youtube.com/playlist?list=PLZgPIJqrkb83DW7sN_dO6sZoxsuu9RhTx

## Games ##
<a href="https://play.google.com/store/apps/details?id=com.aau.jcs" target="_blank">
  <img src="./games/hemlock_logo.png" width="100" height="100" align="middle"/>
</a>
<a href="https://www.youtube.com/watch?v=si_G0zIo0P0&feature=youtu.be" target="_blank">
  <img src="./games/might_&_blade_logo.png" width="232" height="83" align="middle"/>
</a>
<a href="https://mwgamedesign.itch.io/sugar-sleuths" target="_blank">
  <img src="./games/SugarSleuths_logo.png" width="250" height="133" align="middle"/>
</a>
