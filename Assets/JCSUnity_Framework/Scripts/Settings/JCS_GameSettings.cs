/**
 * $File: JCS_GameSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.IO;

namespace JCSUnity
{
    

    public class JCS_GameSettings 
        : MonoBehaviour
    {
        public static JCS_GameSettings instance = null;

        
        
        [Header("** Game Settings **")]
        [SerializeField] public bool DEBUG_MODE = true;
        [SerializeField] public bool THIS_IS_GAME_SCENE = false;
        [SerializeField] public bool LEVEL_DESIGN_MODE = true;
        [SerializeField] public JCS_GameType GAME_TYPE = JCS_GameType.GAME_2D;

        //-- Camera
        [Header("** Camera Settings **")]
        [SerializeField] public JCS_CameraType CAMERA_TYPE = JCS_CameraType.NONE;

        //-- Player
        [Header("** Player Settings **")]
        [Tooltip("Game only allows control one player.")]
        [SerializeField] public bool ACTIVE_ONE_PLAYER = true;
        [Tooltip("Do collusion happen with eacth other. (Player)")]
        [SerializeField] public bool PLAYER_IGNORE_EACH_OTHER = true;
        [Tooltip("Can player damage each other?")]
        [SerializeField] public bool TRIBE_DAMAGE_EACH_OTHER = false;

        [Header("** Platform Settings **")]
        // according to player's character controller's height will
        // should modefied a bit.
        public float GAP_ACCEPT_RANGE = 0.5f;

        [Header("** Scene Settings **")]
        [Tooltip(@"if the object does not set under the 
            JCS_OrderLayer transform this will be activate!")]
        [SerializeField] public int DEFAULT_ORDER_LAYER = 15;

        // no one care about how black screen look so i
        // just make it unseen in the inspector.
        public static string BLACK_SCREEN_PATH = "JCSUnity_Framework_Resources/JCS_LevelDesignUI/JCS_BlackScreen";
        public static string BLACK_SCREEN_NAME = "JCS_BlackScreen";
        public static string WHITE_SCREEN_PATH = "JCSUnity_Framework_Resources/JCS_LevelDesignUI/JCS_WhiteScreen";
        public static string WHITE_SCREEN_NAME = "JCS_WhiteScreen";

        //-- Sound
        public static bool BGM_MUTE = false;
        public static bool EFFECT_MUTE = false;
        public static bool PERFONAL_EFFECT_MUTE = false;

        private static float BGM_SOUND = 0.5f; // Background music [Default: 0.5f]
        private static float SFX_SOUND = 0.5f; // Sound from other player/environment [Default: 0.5f]
        private static float SKILLS_SOUND = 0.5f; // Sound from player [Default: 0.75f]

        //-- Screen Shot
        public static string SCREENSHOT_PATH = "/JCS_ScreenShot/"; // Screen shot folder path [Default: /JCS_ScreenShot/]
        public static string SCREENSHOT_FILENAME = "Screenshot_"; // Screen shot file name [Default: Screenshot_]
        public static string SAVED_IMG_EXTENSION = ".png"; // Extension [Default: .png]

        //-- Game Data Path
        public static string GAME_DATA_PATH = "/JCS_GameData/";
        public static string JCS_EXTENSION = ".jcs";
        
        //-- UI
        [Header("** User Interface Settings **")]
        [SerializeField] public bool RESIZE_UI = true;

        //-- Save Load Settings 
        [Header("** Save Load Settings **")]
        [Tooltip("Save when switching the scene.")]
        [SerializeField] public bool SAVE_ON_SWITCH_SCENE = true;
        [Tooltip("Save when app exit.")]
        [SerializeField] public bool SAVE_ON_EXIT_APP = true;

        public delegate void SavedGameDataDelegate();
        public SavedGameDataDelegate SAVE_GAME_DATA_FUNC = null;

        
        public delegate void LoadGameDataDelegate();    // NOT USED
        public LoadGameDataDelegate LOAD_GAME_DATA_FUNC = null; // NOT USED

        public static JCS_GameData GAME_DATA = null;    // NOT USED


        //--------------------------------
        // setter / getter
        //--------------------------------
        public static float GetBGM_Volume() { return BGM_SOUND; }
        public static void SetBGM_Volume(float volume)
        {
            BGM_SOUND = volume;
            JCS_SoundManager.instance.GetBackgroundMusic().volume = volume;
        }
        public static float GetSFXSound_Volume() { return SFX_SOUND; }
        public static void SetSFXSound_Volume(float volume)
        {
            SFX_SOUND = volume;
            JCS_SoundManager.instance.SetSoundVolume(JCS_SoundSettingType.SFX_SOUND, volume);
        }
        public static void SetSkillsSound_Volume(float volume)
        {
            SKILLS_SOUND = volume;
            JCS_SoundManager.instance.SetSoundVolume(JCS_SoundSettingType.SKILLS_SOUND, volume);
        }
        public static float GetSkillsSound_Volume()
        {
            return SKILLS_SOUND;
        }
        public static float GetSoundBaseOnType(JCS_SoundSettingType type)
        {
            switch (type)
            {
                case JCS_SoundSettingType.BGM_SOUND: return GetBGM_Volume();
                case JCS_SoundSettingType.SFX_SOUND: return GetSFXSound_Volume();
                case JCS_SoundSettingType.SKILLS_SOUND: return GetSkillsSound_Volume();
            }

            JCS_GameErrors.JcsErrors("JCS_GameSetting", -1, "Get unknown volume...");

            return 0;
        }

        //--------------------------------
        // Unity's functions
        //--------------------------------
        private void Awake()
        {
            if (instance != null)
            {
                JCS_GameErrors.JcsWarnings(
                    "JCS_GameSettings", 
                    -1, 
                    "There are too many GameSetting object in the scene. (Delete)");

                TransferData(instance, this);

                // Delete the old one
                DestroyImmediate(instance.gameObject);
            }

            // attach the new one
            instance = this;
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
        private void TransferData(JCS_GameSettings _old, JCS_GameSettings _new)
        {
            // ResizeUI option should always be the same!
            _new.RESIZE_UI = _old.RESIZE_UI;

            // System Settings should always the same.
            _new.SAVE_ON_EXIT_APP = _old.SAVE_ON_EXIT_APP;
            _new.SAVE_ON_SWITCH_SCENE = _old.SAVE_ON_SWITCH_SCENE;

        }

    }
}
