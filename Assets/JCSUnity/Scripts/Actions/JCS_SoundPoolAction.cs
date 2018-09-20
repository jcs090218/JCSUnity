/**
 * $File: JCS_SoundPoolAction.cs $
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
    /// game object itself is the sound player
    /// otherwise use sound pool
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SoundPoolAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        
        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("** Runtime Variables **")]
        [Tooltip("Pool of the audio clips")]
        [SerializeField] private AudioClip[] mAudioClips = null;
        [Tooltip("Sound Type u want to organize")]
        [SerializeField]
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

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
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Randomly play one shot of sound 
        /// from the audio clip pool!
        /// </summary>
        public void PlayRandomSound()
        {
            if (mAudioClips.Length == 0)
                return;

            int randIndex = JCS_Random.Range(0, mAudioClips.Length);

            if (mAudioClips[randIndex] == null)
            {
                JCS_Debug.LogError("JCS_SoundPoolAction",   "You inlcude a null references in he audio pool...");
                return;
            }

            float soundVolume = JCS_SoundSettings.instance.GetSoundBaseOnType(mSoundSettingType);
            mSoundPlayer.PlayOneShot(mAudioClips[randIndex], soundVolume);
        }
        

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
