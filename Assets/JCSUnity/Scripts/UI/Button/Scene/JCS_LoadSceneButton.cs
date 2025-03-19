﻿/**
 * $File: JCS_LoadSceneButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button will load the target scene.
    /// </summary>
    public class JCS_LoadSceneButton : JCS_Button
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_LoadSceneButton)")]

        [Tooltip("Platform you want to target. NONE means all platform, so it will just load the scene")]
        [SerializeField]
        private JCS_PlatformType mPlatformType = JCS_PlatformType.NONE;

        [Tooltip("Scene name you want to load the scene; if empy, load the next scene instead.")]
        [SerializeField]
        private string mSceneName = "";

        [Tooltip("Reload the current scene, and ignore the target scene name.")]
        [SerializeField]
        public bool mReloadScene = false;

        [Tooltip("Screen color when load the scene.")]
        [SerializeField]
        private Color mScreenColor = Color.black;

        [Tooltip("Keep BGM playing when load scene?")]
        [SerializeField]
        private bool mKeppBGM = false;

        /* Setter & Getter */

        public string SceneName { get { return this.mSceneName; } set { this.mSceneName = value; } }
        public bool ReloadScene { get { return this.mReloadScene; } set { this.mReloadScene = value; } }
        public Color ScreenColor { get { return this.mScreenColor; } set { this.mScreenColor = value; } }
        public bool KeppBGM { get { return this.mKeppBGM; } set { this.mKeppBGM = value; } }

        /* Functions */

        public override void OnClick()
        {
            // none meaning all platform so just load the scene.
            if (mPlatformType != JCS_PlatformType.NONE)
            {
                // if the button and the platform are not the same, 
                // dont load the scene and do nothing.
                if (mPlatformType != JCS_AppManager.instance.PlatformType)
                    return;
            }

            string sceneName = JCS_SceneManager.GetSceneNameByOption(mSceneName, mReloadScene);

            JCS_SceneManager.instance.LoadScene(sceneName, mScreenColor, mKeppBGM);
        }
    }
}
