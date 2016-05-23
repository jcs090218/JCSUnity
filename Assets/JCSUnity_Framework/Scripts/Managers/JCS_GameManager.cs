/**
 * $File: JCS_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
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
        public static JCS_GameManager instance = null;

        [SerializeField] public bool GAME_PAUSE = false;

        private JCS_Camera mJCSCamera = null;
        private JCS_Player mJCSPlayer = null;
        private JCS_GameSettings mJCSGameSettings = null;


        //--------------------------------
        // setter / getter
        //--------------------------------
        public void SetJCSCamera(JCS_Camera cam) { this.mJCSCamera = cam; }
        public JCS_Camera GetJCSCamera() { return this.mJCSCamera; }
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
        }

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

