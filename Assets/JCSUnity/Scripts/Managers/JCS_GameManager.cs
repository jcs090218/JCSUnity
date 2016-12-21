/**
 * $File: JCS_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Make sure u have this execute first!!!
    /// </summary>
    public class JCS_GameManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_GameManager instance = null;

        //----------------------
        // Private Variables
        [Header("** Check Variable (JCS_GameManager) **")]

        [Tooltip("Is the game pasue?")]
        public bool GAME_PAUSE = false;

#if (UNITY_EDITOR)

        [Header("** Helper Variable (JCS_GameManager) **")]
        // helper tool for level designer to do 
        // some cool effect in the game.
        [Tooltip("Adjustable current time scale")]
        [SerializeField] [Range(0,1)]
        public float TIME_SCALE = 1;
#endif

        private JCS_Player mJCSPlayer = null;
        private JCS_GameSettings mJCSGameSettings = null;

        //----------------------
        // Protected Variables

        //--------------------------------
        // setter / getter
        //--------------------------------
        public void SetJCSPlayer(JCS_Player player)
        {
            this.mJCSPlayer = player;
        }
        public JCS_Player GetJCSPlayer() { return this.mJCSPlayer; }
        public void SetJCSGameSettings(JCS_GameSettings gs) { this.mJCSGameSettings = gs; }
        public JCS_GameSettings GetJCSGameSettings() { return this.mJCSGameSettings; }

        //--------------------------------
        // Unity's functions
        //--------------------------------
        private void Awake()
        {
            instance = this;
            
            SetSpecificGameTypeGameManager();

#if (UNITY_EDITOR)
            // Check time scale
            if (TIME_SCALE != 1)
            {
                JCS_Debug.JcsReminders(this, 
                    "Current time scale [" + TIME_SCALE + "] isn't one.");
            }
#endif
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            SetTimeScale();
        }

        /// <summary>
        /// Keep set the time to the time scale, 
        /// so make it make the inspector slide bar can 
        /// control the time scale of the actual time scale
        /// from UnityEngine's API "Time class".
        /// </summary>
        private void SetTimeScale()
        {
            Time.timeScale = TIME_SCALE;
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        private void SetSpecificGameTypeGameManager()
        {
            JCS_GameSettings gs = JCS_GameSettings.instance;

            switch (gs.GAME_TYPE)
            {
                case JCS_GameType.GAME_2D:
                        this.gameObject.AddComponent<JCS_2DGameManager>();
                        break;
            }
        }

    }
}

