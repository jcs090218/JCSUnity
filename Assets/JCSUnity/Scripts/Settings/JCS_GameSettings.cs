/**
 * $File: JCS_GameSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.IO;


namespace JCSUnity
{

    /// <summary>
    /// Holde the general game setting.
    /// </summary>
    public class JCS_GameSettings
        : JCS_Settings<JCS_GameSettings>
    {

        //-- 
        [Header("** Game Settings (JCS_GameSettings) **")]
        public bool DEBUG_MODE = true;
        public bool THIS_IS_GAME_SCENE = false;
        public bool LEVEL_DESIGN_MODE = true;
        public JCS_GameType GAME_TYPE = JCS_GameType.GAME_2D;
        public float GRAVITY_PRODUCT = 4.5f;


        //-- Camera
        [Header("** Camera Settings (JCS_GameSettings) **")]
        public JCS_CameraType CAMERA_TYPE = JCS_CameraType.NONE;


        //-- Player
        [Header("** Player Settings (JCS_GameSettings) **")]
        [Tooltip("Game only allows control one player.")]
        public bool ACTIVE_ONE_PLAYER = true;

        //-- 
        [Header("** Collision Settings **")]

        [Tooltip("Do collusion happen with eacth other. (Player)")]
        public bool PLAYER_IGNORE_EACH_OTHER = true;

        [Tooltip("Can tribe damage each other?")]
        public bool TRIBE_DAMAGE_EACH_OTHER = false;

        [Tooltip("Careful, this will override player ignore options!")]
        public bool IGNORE_EACH_OTHER_CHARACTER_CONTROLLER = true;


        [Header("** Scene Settings (JCS_GameSettings) **")]

        // no one care about how black screen look so i
        // just make it unseen in the inspector.
        public static string BLACK_SCREEN_PATH = "JCSUnity_Resources/JCS_LevelDesignUI/JCS_BlackScreen";
        public static string BLACK_SLIDE_SCREEN_PATH = "JCSUnity_Resources/JCS_LevelDesignUI/JCS_BlackSlideScreen";
        public static string BLACK_SCREEN_NAME = "JCS_BlackScreen";
        public static string WHITE_SCREEN_PATH = "JCSUnity_Resources/JCS_LevelDesignUI/JCS_WhiteScreen";
        public static string WHITE_SCREEN_NAME = "JCS_WhiteScreen";

        //-- Screen Shot
        public string SCREENSHOT_PATH = "/JCS_ScreenShot/"; // Screen shot folder path [Default: /JCS_ScreenShot/]
        public string SCREENSHOT_FILENAME = "Screenshot_"; // Screen shot file name [Default: Screenshot_]
        public string SAVED_IMG_EXTENSION = ".png"; // Extension [Default: .png]

        //-- Game Data Path
        public static string GAME_DATA_PATH = "/JCS_GameData/";
        public static string JCS_EXTENSION = ".jcs";


        //-- UI
        [Header("** User Interface Settings (JCS_GameSettings) **")]
        public bool RESIZE_UI = true;


        [Header("** Save Load Settings (JCS_GameSettings) **")]

        [Tooltip("Save when switching the scene.")]
        public bool SAVE_ON_SWITCH_SCENE = true;

        [Tooltip("Save when app exit.")]
        public bool SAVE_ON_EXIT_APP = true;

        public delegate void SavedGameDataDelegate();
        public SavedGameDataDelegate SAVE_GAME_DATA_FUNC = null;


        public delegate void LoadGameDataDelegate();    // NOT USED
        public LoadGameDataDelegate LOAD_GAME_DATA_FUNC = null; // NOT USED

        public static JCS_XMLGameData GAME_DATA = null;    // NOT USED


        [Header("** Damage Settings (JCS_GameSettings) **")]

        [Tooltip("Mininum damage can be in the game.")]
        public int MIN_DAMAGE = 1;

        [Tooltip("Maxinum damage can be in the game.")]
        public int MAX_DAMAGE = 999999;


        //--------------------------------
        // setter / getter
        //--------------------------------

        //--------------------------------
        // Unity's functions
        //--------------------------------
        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void Start()
        {
            JCS_GameWindowHandler gwh = JCS_GameWindowHandler.instance;

            // if this is the game scene, 
            // enable the game ui.
            if (THIS_IS_GAME_SCENE)
            {
                if (gwh != null)
                    gwh.ShowGameUI();
            }
            // if this is NOT the game scene, 
            // dis-enable the game ui.
            else
            {
                if (gwh != null)
                    gwh.HideGameUI();
            }
        }

        private void Update()
        {
            JCS_Input.MouseDeltaPosition();
        }

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_GameSettings _old, JCS_GameSettings _new)
        {
            // ResizeUI option should always be the same!
            _new.RESIZE_UI = _old.RESIZE_UI;

            // System Settings should always the same.
            _new.SAVE_ON_EXIT_APP = _old.SAVE_ON_EXIT_APP;
            _new.SAVE_ON_SWITCH_SCENE = _old.SAVE_ON_SWITCH_SCENE;

        }

    }
}
