/**
 * $File: JCS_LoadSceneButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    /// <summary>
    /// Button will load the scene.
    /// </summary>
    public class JCS_LoadSceneButton
        : JCS_Button
    {
        [Header("** Initialize Variables (JCS_LoadSceneButton) **")]
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

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public override void JCS_ButtonClick()
        {
            // none meaning all platform so just load the scene.
            if (mPlatformType != JCS_PlatformType.NONE)
            {
                // if the button and the platform are not the same, 
                // dont load the scene and do nothing.
                if (mPlatformType != JCS_ApplicationManager.instance.PLATFORM_TYPE)
                    return;
            }

            // do call back
            base.JCS_ButtonClick();

            JCS_SceneManager.instance.LoadScene(mSceneName, mScreenColor);
        }
    }
}
