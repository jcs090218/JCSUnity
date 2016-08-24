/**
 * $File: JCS_WaitDestroySoundEndEvent.cs $
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
    /// Careful this won't destroy it self!!!
    /// </summary>
    [RequireComponent(typeof(JCS_DestroyReminder))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_WaitDestroySoundEndEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private bool mSoundPlayed = false;
        private JCS_SoundPlayer mSoundPlayer = null;

        [SerializeField]
        private AudioClip mAudioClip = null;
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetSoundSettingType(JCS_SoundSettingType type) { mSoundSettingType = type; }
        public void SetAudioClip(AudioClip ac) { this.mAudioClip = ac; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        private void Update()
        {
            if (mSoundPlayed)
                return;


            mSoundPlayer.PlayOneShot(mAudioClip, mSoundSettingType);

            mSoundPlayed = true;
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
