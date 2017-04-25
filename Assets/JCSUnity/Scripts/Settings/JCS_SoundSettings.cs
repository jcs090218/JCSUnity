/**
 * $File: JCS_SoundSettings.cs $
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
    /// Sound Setting for each theme.
    /// </summary>
    public class JCS_SoundSettings
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_SoundSettings instance = null;

        //----------------------
        // Private Variables

        [Header("** Scene Sound **")]
        [SerializeField]
        public bool SMOOTH_SWITCH_SOUND_BETWEEN_SCENE = true;
        [SerializeField]
        public AudioClip BACKGROUND_MUSIC = null;

        // Window System
        [Header("** Window System **")]
        [SerializeField]
        public AudioClip DEFAULT_OPEN_WINDOW_CLIP = null;
        [SerializeField]
        public AudioClip DEFAULT_CLOSE_WINDOW_CLIP = null;


        [Header("** Runtime Variables (JCS_SoundSettings) **")]

        [Tooltip("General Sound fadout time.")]
        public float SOUND_FADEOUT_TIME = 1.5f;

        [Tooltip("General Sound fadein time.")]
        public float SOUND_FADEIN_TIME = 1.5f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Get the real sound fade out time base on 
        /// the sound manager override setting.
        /// </summary>
        /// <returns> timeto fade out the music. </returns>
        public float GetSoundFadeOutTimeBaseOnSetting()
        {
            JCS_SoundManager jcsSm = JCS_SoundManager.instance;

            // check if override the setting.
            if (jcsSm.OverrideSetting)
            {
                // return the override value.
                return jcsSm.SoundFadeOutTime;
            }

            // if not override, 
            // return the basic value.
            return SOUND_FADEOUT_TIME;
        }

        /// <summary>
        /// Get the real sound fade in time base on
        /// the sound manager override setting?
        /// </summary>
        /// <returns> time to fade in the sound </returns>
        public float GetSoundFadeInTimeBaseOnSetting()
        {
            JCS_SoundManager jcsSm = JCS_SoundManager.instance;

            // check if override the setting.
            if (jcsSm.OverrideSetting)
            {
                // return the override value.
                return jcsSm.SoundFadeInTime;
            }

            // if not override, 
            // return the basic value.
            return SOUND_FADEIN_TIME;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
