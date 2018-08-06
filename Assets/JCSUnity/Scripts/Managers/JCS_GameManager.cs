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
        : JCS_Managers<JCS_GameManager>
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_GameManager) **")]

        [Tooltip("Test this module?")]
        [SerializeField]
        private bool mTestWithKey = false;
#endif


        [Header("** Check Variable (JCS_GameManager) **")]

        [Tooltip("Is the game pasue?")]
        [SerializeField]
        private bool mGamePause = false;


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
        public bool GAME_PAUSE
        {
            get { return this.mGamePause; }
            set
            {
                // check if need the game pause the same as the value
                // previously set. In order to save some perforce by 
                // enable/disable all the JCS_PauseAction in the game.
                if (mGamePause != value)
                {
                    JCS_PauseManager.instance.PauseTheWholeGame(value);
                }

                this.mGamePause = value;
            }
        }
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
                JCS_Debug.LogReminders( 
                    "Current time scale [" + TIME_SCALE + "] isn't one.");
            }
#endif
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            SetTimeScale();

            TestPauseGame();
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

        private void TestPauseGame()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(KeyCode.P))
            {
                GAME_PAUSE = !GAME_PAUSE;
            }
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

