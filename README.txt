========================================================================
$File: README.txt $
$Creator: Jen-Chieh Shen <jcs090218@gmail.com> $
$Date: 2016-03-29 $
$Revision: 1.4.1 $
$Version Control Page: https://github.com/jcs090218/JCSUnity_Framework $
$Unity Version: 5.5.1f1 (64-bit) $
$Notice: See LICENSE.txt for modification and distribution information 
                  Copyright (c) 2016 by Shen, Jen-Chieh $
========================================================================


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-=      Acknowledge
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    Demo Scene are all avialiable in "Assets/JCSUnity_Framework/Scenes"
    
    Demo Video is at this link:
    * https://www.youtube.com/playlist?list=PLZgPIJqrkb83SBfBSzk0SMchegZFO9lKI


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Libraries List
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    Can be found in "doc/JCSUnity Libraries List.txt".
    
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Execution Order List
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    Can be found in "doc/JCSUnity Execution Order List.txt".

    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Naming Explaination
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    Can be found in "doc/JCSUnity Naming Manual.txt".

    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Level Design Rule 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Every Scene must need "JCS_Managers" and "JCS_Settings" object in the "Hierarchy"
    * Use Start Scene in the Folder "Assets/JCSUnity/Scenes/StarterScene"


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-=      System Explaination
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Animator System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Using Integer to represent the state! 
        (see "JCS_PlayerState" have more detail, 
        if just a animation without changing use IDLE!)

        
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Sound System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    (Level Designer)
    * Sound related variables can be set at "JCS_SoundSettings" script at "JCS_Settings" object!
    * Add BGM plz add "JCS_BGMPlayer" on Camera object.

    (Scripter)
    * Anyone who want to call the effect of switching the scene should call "LoadScene(string)" 
     in "JCS_SceneManager" script
        
        
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Dialogue System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
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
        
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= GUI 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Liquid Bar 
        - 請參考spr_sao_health_bar_under的美術製作方法. (那個bar)
        
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Logo System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Logo Screen relate variables can be set at "JCS_Logo" script in "JCS_LogoScene"!


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Network System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Enable/Disable socket can be set at "JCS_ApplicationManager" script.
    * Host Name, Port and relate variables can be set at "JCS_NetworkConstant" script.


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Player System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    //-- Jump --//
    * Set "JumpType" accordingly to "Jump Forces"'s size (Maximize to Triple Jump = 3)
    Basic Jump = size 1;
    Double Jump = size 2;
    Triple Jump = size 3;


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Scene System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * If is not a online game you can ignore the scene "JCS_Patcher", can directly go into Logo Scene.


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Screen Shot System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Screen shot path, can be set at "JCS_GameSetting" script under Screen Shot section
    * Screen shot file name, can be set at "JCS_GameSetting" script under Screen Shot section
    * Screen shot extension, can be set at "JCS_GameSetting" script under Screen Shot section
    * function call "JCS_2DCamera->TakeScreenShot"


    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    =-= Sprite System 
    =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    * Defualt Sprite is facing left so make sure the sprite you are using default are facing left.
    
