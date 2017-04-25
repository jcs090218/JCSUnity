/**
 * $File: JCS_TriggerSwitchBGMEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Trigger switch the bgm.
    /// </summary>
    public class JCS_TriggerSwitchBGMEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables


        [Header("** Runtime Variables (JCS_TriggerSwitchBGMEvent) **")]

        [Tooltip("Sound you want to load.")]
        [SerializeField]
        private AudioClip mSoundClip = null;

        [Tooltip("Time to fade in the sound.")]
        [SerializeField]
        private float mSoundFadeInTime = 1;

        [Tooltip("Time to fade out the sound.")]
        [SerializeField]
        private float mSoundFadeOutTime = 1;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void OnTriggerEnter(Collider other)
        {
            JCS_SoundManager.instance.SwitchBackgroundMusic(
                mSoundClip, 
                mSoundFadeInTime, 
                mSoundFadeOutTime);
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
