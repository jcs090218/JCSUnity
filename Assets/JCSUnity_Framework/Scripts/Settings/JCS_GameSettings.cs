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
    public class JCS_GameSettings : MonoBehaviour
    {
        public static JCS_GameSettings instance = null;

        [Header("** Game Settings **")]
        public static bool DEBUG_MODE = true;
        [SerializeField] public bool THIS_IS_GAME_SCENE = false;
        [SerializeField] public bool LEVEL_DESIGN_MODE = true;
        [SerializeField] public JCS_GameType GAME_TYPE = JCS_GameType.GAME_2D;

        //-- Camera
        [Header("** Camera Settings **")]
        [SerializeField] public JCS_CameraType CAMERA_TYPE = JCS_CameraType.NONE;

        //-- Player
        [Header("** Player Settings **")]
        [SerializeField] public bool PLAYER_IGNORE_EACH_OTHER = true;

        //-- Canvas
        [Header("** Canvas Settings **")]
        [SerializeField] public string GAME_UI_PATH = "JCSUnity_Framework_Resources/JCS_InGameDialogue/JCS_InGameUI";
        public static string GAME_UI_NAME = "JCS_InGameUI";

        // no one care about how black screen look so i
        // just make it unseen in the inspector.
        public static string BLACK_SCREEN_PATH = "JCSUnity_Framework_Resources/JCS_LevelDesignUI/JCS_BlackScreen";
        public static string BLACK_SCREEN_NAME = "JCS_BlackScreen";

        //-- Sound
        public static bool BGM_MUTE = false;
        public static bool EFFECT_MUTE = false;
        public static bool PERFONAL_EFFECT_MUTE = false;

        private static float BGM_SOUND = 0.5f; // Background music [Default: 0.5f]
        private static float SFX_SOUND = 0.5f; // Sound from other player/environment [Default: 0.5f]
        private static float SKILLS_SOUND = 0.75f; // Sound from player [Default: 0.75f]

        //-- Screen Shot
        public static string SCREENSHOT_PATH = "/JCS_ScreenShot/"; // Screen shot folder path [Default: /JCS_ScreenShot/]
        public static string SCREENSHOT_FILENAME = "Screenshot_"; // Screen shot file name [Default: Screenshot_]
        public static string SAVED_IMG_EXTENSION = ".png"; // Extension [Default: .png]

        //-- UI
        [Header("** User Interface Settings **")]
        [SerializeField] public bool RESIZE_UI = true;

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

        //--------------------------------
        // Unity's functions
        //--------------------------------
        private void Awake()
        {
            if (instance != null)
            {
                JCS_GameErrors.JcsErrors("JCS_GameSettings", -1, "There are too many GameSetting object in the scene. (Delete)");

                TransferData(instance, this);

                // Delete the old one
                DestroyImmediate(instance.gameObject);
            }

            // attach the new one
            instance = this;

            GetGameData();
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

        }

        private void GetGameData()
        {
            // Game UI path,
            // 
            GAME_UI_NAME = Path.GetFileNameWithoutExtension(GAME_UI_PATH);
        }

    }
}
