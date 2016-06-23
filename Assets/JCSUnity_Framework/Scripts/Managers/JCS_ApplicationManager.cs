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

    public class JCS_ApplicationManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_ApplicationManager instance = null;
        public static bool APP_PAUSE = false;
        public static bool ONLINE_MODE = false;
        public static bool APP_QUITTING = false;

        public static bool FIRST_LOGIN = true;

        [Header("** This will override Platform Type (Uncheck this when you want to release.)**")]
        public bool SIMULATE_PLATFORM_TYPE = true;
        public JCS_PlatformType PLATFORM_TYPE = JCS_PlatformType.PC;

        //----------------------
        // Private Variables
        private void SetPlatformType(JCS_PlatformType pt)
        {
            if (SIMULATE_PLATFORM_TYPE)
                return;

            PLATFORM_TYPE = pt;
        }

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
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
        }

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

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public bool isPC()
        {
            return (PLATFORM_TYPE == JCS_PlatformType.PC);
        }
        public bool isMobile()
        {
            return (PLATFORM_TYPE == JCS_PlatformType.MOBILE);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions


    }
}
