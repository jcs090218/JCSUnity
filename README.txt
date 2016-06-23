Name: Jen-Chieh Shen <lkk440456@gmail.com>
File Name: README.txt
Project Version: 1.3.3
Version Control Page: https://github.com/mike316mike316/JCSUnity_Framework.git
Unity Version: 5.3.5f1 (64-bit)
Last Edit Date: 2016-05-23
-----------------------------------

//==================================
//      Acknowledge
//----------------------------
Demo Scene are all avialiable in "Assets/JCSUnity_Framework/Scenes"

####################################
### Execution Order Explaination ###
####################################
Following Scirpt should have lower execute order then "JCS_GameManager"
(Notice: "Edit->Project Settings->Script Execution Order" to set the execution order!)

* JCS_Camera (include all the sub-classes)
* JCS_MobileMouseEvent
* JCS_Canvas


#########################
### Level Design Rule ###
#########################
- Every Scene must need "JCS_Managers" and "JCS_Settings" object in the "Hierarchy"


//==================================
//      System Explaination
//----------------------------
#######################
### Animator System ###
#######################
* Using Integer to represent the state! 
    (see "JCS_PlayerState" have more detail, 
    if just a animation without changing use IDLE!)

    
####################
### Sound System ###
####################
(Level Designer)
* Sound related variables can be set at "JCS_SoundSettings" script at "JCS_Settings" object!

(Scripter)
* Anyone who want to call the effect of switching the scene should call "LoadScene(string)" 
 in "JCS_SceneManager" script
    
    
#######################
### Dialogue System ###
#######################
(Level Designer)
* Create Dialogue and attach "JCS_DialogueObject" script
* After modefied the whole scene pull it into Resource folder so in order to load.
* all the default path are able to modeified at "JCS_ButtonFunctions" script
* Force Dialogue - Dialogue depend on application layer
* In Game Dialogue - Dialogue depend on game layer

* How to spawn panel?
    1) Create Empty Game Object
    2) Attach "JCS_GameWindowHandler" script
    3) set the variable it will do.

* How to make it Drag and Drop panel?
    1) Attach "JCS_DragDropObject" script
    2) Add "Event Trigger" component
    3) use "Event Trigger" add the drag event and pull the object into the appearing slot.
    4) choose the function call in the script
    
###########
### GUI ###
###########
* Liquid Bar 
    - 請參考spr_sao_health_bar_under的美術製作方法. (那個bar)
    
###################
### Logo System ###
###################
* Logo Screen relate variables can be set at "JCS_Logo" script in "JCS_LogoScene"!


######################
### Network System ###
######################
* Enable/Disable socket can be set at "JCS_ApplicationManager" script.
* Host Name, Port and relate variables can be set at "JCS_NetworkConstant" script.


#####################
### Player System ###
#####################
//-- Jump --//
* Set "JumpType" accordingly to "Jump Forces"'s size (Maximize to Triple Jump = 3)
Basic Jump = size 1;
Double Jump = size 2;
Triple Jump = size 3;


####################
### Scene System ###
####################
* If is not a online game you can ignore the scene "JCS_Patcher", can directly go into Logo Scene.


##########################
### Screen Shot System ###
##########################
* Screen shot path, can be set at "JCS_GameSetting" script under Screen Shot section
* Screen shot file name, can be set at "JCS_GameSetting" script under Screen Shot section
* Screen shot extension, can be set at "JCS_GameSetting" script under Screen Shot section
* function call "JCS_2DCamera->TakeScreenShot"


#####################
### Sprite System ###
#####################
* Defualt Sprite is facing left so make sure the sprite you are using default are facing left.

