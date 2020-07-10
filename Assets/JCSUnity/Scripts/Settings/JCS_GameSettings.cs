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
    public delegate void SavedGameDataDelegate();
    public delegate void LoadGameDataDelegate();    // NOT USED

    /// <summary>
    /// Hold the general game setting.
    /// </summary>
    public class JCS_GameSettings
        : JCS_Settings<JCS_GameSettings>
    {
        /* Variables */

        public SavedGameDataDelegate SAVE_GAME_DATA_FUNC = null;
        public LoadGameDataDelegate LOAD_GAME_DATA_FUNC = null; // NOT USED

        public static JCS_XMLGameData GAME_DATA = null;    // NOT USED

        [Header("** Game Settings (JCS_GameSettings) **")]

        [Tooltip("Debug mode flag.")]
        public bool DEBUG_MODE = true;

        [Tooltip("Game scene flag.<")]
        public bool THIS_IS_GAME_SCENE = false;

        [Tooltip("Level design mode flag.")]
        public bool LEVEL_DESIGN_MODE = true;

        [Tooltip("Game type flag.")]
        public JCS_GameType GAME_TYPE = JCS_GameType.GAME_2D;

        [Tooltip("Gravity production. (For game that have gravity in it)")]
        public float GRAVITY_PRODUCT = 4.5f;

        [Header("- Camera")]

        [Tooltip("Type of the camera.")]
        public JCS_CameraType CAMERA_TYPE = JCS_CameraType.NONE;

        [Header("- Player")]

        [Tooltip("Game only allows control one player.")]
        public bool ACTIVE_ONE_PLAYER = true;

        [Header("- Collision")]

        [Tooltip("Do collusion happen with eacth other. (Player)")]
        public bool PLAYER_IGNORE_EACH_OTHER = true;

        [Tooltip("Can tribe damage each other?")]
        public bool TRIBE_DAMAGE_EACH_OTHER = false;

        [Tooltip("Careful, this will override player ignore options!")]
        public bool IGNORE_EACH_OTHER_CHARACTER_CONTROLLER = true;

        [Header("- Save Load")]

        [Tooltip("Data folder path.")]
        public string DATA_PATH = "/JCS_GameData/";

        [Tooltip("Data file extension.")]
        public string DATA_EXTENSION = ".jcs";

        [Tooltip("Save when switching the scene.")]
        public bool SAVE_ON_SWITCH_SCENE = true;

        [Tooltip("Save when app exit.")]
        public bool SAVE_ON_EXIT_APP = true;

        [Header("- Screenshot")]
        
        [Tooltip("Screenshot folder path.")]
        public string SCREENSHOT_PATH = "/JCS_GameData/Screenshot/";

        [Tooltip("Screenshot file name.")]
        public string SCREENSHOT_FILENAME = "Screenshot_";

        [Tooltip("Screenshot image extension.")]
        public string SCREENSHOT_EXTENSION = ".png";

        [Header("- Webcam")]

        [Tooltip("Webcam image save path.")]
        public string WEBCAM_PATH = "/JCS_GameData/WebcamShot/";

        [Tooltip("Webcam file name.")]
        public string WEBCAM_FILENAME = "";

        [Tooltip("Webcam image extension.")]
        public string WEBCAM_EXTENSION = ".png";

        [Header("- Damage")]

        [Tooltip("Mininum damage can be in the game.")]
        public int MIN_DAMAGE = 1;

        [Tooltip("Maxinum damage can be in the game.")]
        public int MAX_DAMAGE = 999999;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);

            Directory.CreateDirectory(JCS_GameData.SavePath());
            Directory.CreateDirectory(JCS_Camera.SavePath());
            Directory.CreateDirectory(JCS_Webcam.SavePath());
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
            // Debug check
            _new.DEBUG_MODE = _old.DEBUG_MODE;

            // System Settings should always the same.
            _new.SAVE_ON_EXIT_APP = _old.SAVE_ON_EXIT_APP;
            _new.SAVE_ON_SWITCH_SCENE = _old.SAVE_ON_SWITCH_SCENE;

        }
    }
}
