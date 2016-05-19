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

            GetPlatformGap(player);
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
        }

        /// <summary>
        /// Get the gap between player and platform
        /// 
        /// Gap 的長度等於CharacterController's height的一半
        /// </summary>
        /// <param name="player"></param>
        private void GetPlatformGap(JCS_Player player)
        {
            if (player == null)
                return;

            JCS_GameSettings.instance.PLATFORM_AND_PLAYER_GAP = player.GetCharacterController().height / 2;
        }

    }
}

