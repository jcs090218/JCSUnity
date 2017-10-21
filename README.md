# JCSUnity #
<br/>

JCSUnity is a general speed components framework. JCSUnity help out
people who have trouble making script/component over and over agian
in variety of different project. This framework provide many scripted
game mechanics. With those combination of scripts/components could
generate enormous variety of different behaviour. <br/><br/>

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

### Demo ###
* https://www.youtube.com/playlist?list=PLZgPIJqrkb83SBfBSzk0SMchegZFO9lKI
