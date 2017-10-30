/**
 * $File: JCS_LoadSceneGamePadButton.cs $
 * $Date: 2017-10-27 11:44:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Button will load the scene. (Game Pad)
    /// </summary>
    public class JCS_LoadSceneGamePadButton
        : JCS_GamePadButton
    {
        [Header("** Initialize Variables (JCS_LoadSceneGamePadButton) **")]

        [Tooltip("Platform u want to target. NONE means all platform, so it will just load the scene")]
        [SerializeField]
        private JCS_PlatformType mPlatformType = JCS_PlatformType.NONE;

        [Tooltip("Scene u want to load after clicking this button.")]
        [SerializeField]
        private string mSceneName = "JCS_ApplicationCloseSimulateScene";

        [Tooltip("Screen to fade.")]
        [SerializeField]
        private Color mScreenColor = Color.black;

        //========================================
        //      setter / getter
        //------------------------------
        public string SceneName { get { return this.mSceneName; } set { this.mSceneName = value; } }
        public Color ScreenColor { get { return this.mScreenColor; } set { this.mScreenColor = value; } }

        //========================================
        //      Self-Define
        //------------------------------

        public override void JCS_OnClickCallback()
        {
            // none meaning all platform so just load the scene.
            if (mPlatformType != JCS_PlatformType.NONE)
            {
                // if the button and the platform are not the same, 
                // dont load the scene and do nothing.
                if (mPlatformType != JCS_ApplicationManager.instance.PLATFORM_TYPE)
                    return;
            }

            JCS_SceneManager.instance.LoadScene(mSceneName, mScreenColor);
        }
    }
}
