/**
 * $File: JCS_SoundSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    public class JCS_SoundSettings : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_SoundSettings instance = null;

        //----------------------
        // Private Variables

        [Header("** Scene Sound **")]
        [SerializeField] public bool SMOOTH_SWITCH_SOUND_BETWEEN_SCENE = true;
        [SerializeField] public AudioClip BACKGROUND_MUSIC = null;

        // Window System
        [Header("** Window System **")]
        [SerializeField] public AudioClip DEFAULT_OPEN_WINDOW_CLIP = null;
        [SerializeField] public AudioClip DEFAULT_CLOSE_WINDOW_CLIP = null;

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

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
