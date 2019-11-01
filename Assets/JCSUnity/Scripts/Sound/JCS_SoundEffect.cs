/**
 * $File: JCS_SoundEffect.cs $
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
    /// Sound Effect use for environment!
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_SoundEffect : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private AudioSource mAudioSource = null;

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
            this.mAudioSource = this.GetComponent<AudioSource>();

            // Check to see if there is audio attach in the game or not!
            if (mAudioSource.clip == null)
                JCS_Debug.LogError("Sound Effect Object with out audio clip init...");
        }

        private void Start()
        {
            // add sound to let "SoundManager" take care of all the effect
            JCS_SoundManager.instance.AssignSoundSource(JCS_SoundSettingType.SFX_SOUND, mAudioSource);
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
