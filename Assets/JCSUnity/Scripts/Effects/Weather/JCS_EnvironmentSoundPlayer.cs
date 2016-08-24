/**
 * $File: JCS_EnvironmentSoundPlayer.cs $
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

    public class JCS_EnvironmentSoundPlayer
        : JCS_SoundPlayer
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Tooltip("Careful this does not effect to any sound system.")]
        [SerializeField] private AudioClip mEnvironmentSound = null;

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

            GetAudioSource().loop = true;

            // NOTE(JenChieh): does not connect to bgm yet.
            GetAudioSource().clip = mEnvironmentSound;
            GetAudioSource().Play();
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
