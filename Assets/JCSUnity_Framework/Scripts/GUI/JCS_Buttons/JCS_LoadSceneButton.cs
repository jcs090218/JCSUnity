/**
 * $File: JCS_LoadSceneButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    public class JCS_LoadSceneButton
        : JCS_Button
    {
        [Header("** Initialize Variables (JCS_LoadSceneButton) **")]
        [Tooltip("Platform u want to target. NONE means all platform, so it will just load the scene")]
        [SerializeField] private JCS_PlatformType mPlatformType = JCS_PlatformType.NONE;

        [Tooltip("Scene u want to load after clicking this button.")]
        [SerializeField] private string mSceneName = "JCS_ApplicationCloseSimulateScene";


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

            JCS_SceneManager.instance.LoadScene(mSceneName);

            // do call back
            base.JCS_ButtonClick();
        }
    }
}
