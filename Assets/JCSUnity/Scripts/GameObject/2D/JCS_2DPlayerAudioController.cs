/**
 * $File: JCS_2DPlayerAudioController.cs $
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
    /// Audio player for specific player type.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_2DPlayerAudioController 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        protected AudioListener mAudioListener = null;

        // Audio Source equals to Audio Player!
        protected JCS_SoundPlayer mJCSSoundPlayer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public AudioListener GetAudioListener() { return this.mAudioListener; }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            mJCSSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            mAudioListener = this.GetComponent<AudioListener>();
        }

        protected virtual void Start()
        {
            JCS_SoundManager.instance.SetAudioListener(GetAudioListener());

            // add it to Sound Manager so it could manage
            // the volume and mute!
            JCS_SoundManager.instance.AssignSoundSource(JCS_SoundSettingType.SKILLS_SOUND, mJCSSoundPlayer.GetAudioSource());
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
