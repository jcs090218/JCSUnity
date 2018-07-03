/**
 * $File: JCS_BGMPlayer.cs $
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
    /// Sound Player that plays BGM.
    /// </summary>
    public class JCS_BGMPlayer 
        : JCS_SoundPlayer
    {

        //----------------------
        // Public Variables

        public static JCS_BGMPlayer instance = null;

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
        protected override void Awake()
        {
            base.Awake();

            // NOTE(jenchieh): Only the first time will call this.
            // This game object is a unique game object. Meaning the
            // object itself uses 'DontDestroyOnLoad' function.
            if (instance == null)
            {
                instance = this;

                // ==> OnLevelWasLoaded <==
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
                {
                    if (!JCS_SoundSettings.instance.KEEP_BGM_SWITCH_SCENE)
                    {
                        // set to Sound Manager in order to get manage
                        JCS_SoundManager.instance.SetBackgroundMusic(GetAudioSource());

                        // Assign BGM from Sound Manager!
                        GetAudioSource().clip = JCS_SoundSettings.instance.BACKGROUND_MUSIC;

                        GetAudioSource().Play();
                    }
                    else
                    {
                        // If the keep bgm is true, we disable it once 
                        // everytime a scene is loaded.
                        JCS_SoundSettings.instance.KEEP_BGM_SWITCH_SCENE = false;
                    }
                };
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
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
