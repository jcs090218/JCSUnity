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
        /* Variables */

        public static JCS_BGMPlayer instance = null;

        /* Setter & Getter */

        /* Functions */

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
                    // set to Sound Manager in order to get manage
                    JCS_SoundManager.instance.SetBackgroundMusic(GetAudioSource());

                    if (!JCS_SoundSettings.instance.KEEP_BGM_SWITCH_SCENE)
                    {
                        // Assign BGM from Sound Manager!
                        GetAudioSource().clip = JCS_SoundSettings.instance.BACKGROUND_MUSIC;

                        GetAudioSource().Play();
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
    }
}
