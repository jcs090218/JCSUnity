/**
 * $File: JCS_ApplicationManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// Manager manage application layer.
    /// </summary>
    public class JCS_ApplicationManager 
        : JCS_Managers<JCS_ApplicationManager>
    {

        //----------------------
        // Public Variables

        public static bool APP_PAUSE = false;
        public static bool APP_QUITTING = false;
        public static bool APP_INITIALIZING = true;


        [Header("** Runtime Variables (JCS_ApplicationManager) **")]

        [Tooltip("This will override Platform Type.")]
        public bool SIMULATE_PLATFORM_TYPE = true;

        [Tooltip("Target platform type to simulate.")]
        public JCS_PlatformType PLATFORM_TYPE = JCS_PlatformType.PC;

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        private void SetPlatformType(JCS_PlatformType pt)
        {
            if (SIMULATE_PLATFORM_TYPE)
                return;

            PLATFORM_TYPE = pt;
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void OnApplicationQuit()
        {
            APP_QUITTING = true;

            JCS_GameSettings gs = JCS_GameSettings.instance;
            if (gs.SAVE_ON_EXIT_APP &&
                gs.SAVE_GAME_DATA_FUNC != null)
            {
                // save when exit app
                gs.SAVE_GAME_DATA_FUNC.Invoke();
            }
        }

        private void Awake()
        {
            instance = this;

#if (UNITY_STANDALONE || UNITY_EDITOR)
            // set platform type
            SetPlatformType(JCS_PlatformType.PC);

#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            // set platform type
            SetPlatformType(JCS_PlatformType.MOBILE);

            // set the sleep time to never
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

            // done initialize the application layer.
            APP_INITIALIZING = false;
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        /// <summary>
        /// Is platform pc?
        /// </summary>
        /// <returns>
        /// true: is pc,
        /// false: not pc.
        /// </returns>
        public bool isPC()
        {
            return (PLATFORM_TYPE == JCS_PlatformType.PC);
        }

        /// <summary>
        /// Check the platform mobile?
        /// </summary>
        /// <returns>
        /// ture: is mobile,
        /// false: not mobile
        /// </returns>
        public bool isMobile()
        {
            return (PLATFORM_TYPE == JCS_PlatformType.MOBILE);
        }

        /// <summary>
        /// Quit the application
        /// </summary>
        public void Quit(bool fade = true)
        {
            if (fade)
            {
                // load the quit scene.
                JCS_SceneManager.instance.LoadScene("JCS_ApplicationCloseSimulateScene");
            }
            else
            {
                // quit the app directly,
                Application.Quit();
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions


    }
}
