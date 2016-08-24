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

    public class JCS_BGMPlayer 
        : JCS_SoundPlayer
    {

        //----------------------
        // Public Variables

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
        protected override void Start()
        {
            base.Start();

            // if there is no bgm sound use bgm from setting!
            // assume designer can still override this!
            if (GetAudioSource().clip == null)
            {
                // Assign BGM from Sound Manager!
                GetAudioSource().clip = JCS_SoundSettings.instance.BACKGROUND_MUSIC;


                // set to Sound Manager in order to get manage
                JCS_SoundManager.instance.SetBackgroundMusic(GetAudioSource());

                GetAudioSource().Play();
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
