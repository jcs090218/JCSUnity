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
    public class JCS_ApplicationManager : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_ApplicationManager instance = null;
        public static bool APP_PAUSE = false;
        public static bool ONLINE_MODE = false;

        public static bool FIRST_LOGIN = true;

        //----------------------
        // Private Variables

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

#if (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions


    }
}
