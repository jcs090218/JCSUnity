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
        : JCS_Settings<JCS_SoundSettings>
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Settings (JCS_SoundSettings) **")]

        [Tooltip("Background Music Mute?")]
        public bool BGM_MUTE = false;
        [Tooltip("SFX mute?")]
        public bool EFFECT_MUTE = false;
        [Tooltip("player sound effect mute?")]
        public bool PERFONAL_EFFECT_MUTE = false;

        [Tooltip("Background music [Default: 0.5f]")]
        [Range(0, 1)]
        public float BGM_SOUND = 0.4f;

        [Tooltip("Sound from other player/environment [Default: 0.5f]")]
        [Range(0, 1)]
        public float SFX_SOUND = 0.4f;

        [Tooltip("Sound from player [Default: 0.75f]")]
        [Range(0, 1)]
        public float SKILLS_SOUND = 0.4f;


        [Header("** Scene Sound (JCS_SoundSettings) **")]

        [Tooltip("")]
        public bool SMOOTH_SWITCH_SOUND_BETWEEN_SCENE = true;

        [Tooltip("")]
        public AudioClip BACKGROUND_MUSIC = null;


        // Window System
        [Header("** Window System (JCS_SoundSettings) **")]

        [Tooltip("")]
        public AudioClip DEFAULT_OPEN_WINDOW_CLIP = null;

        [Tooltip("")]
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
        public float GetBGM_Volume() { return BGM_SOUND; }
        public void SetBGM_Volume(float volume)
        {
            BGM_SOUND = volume;
            JCS_SoundManager.instance.GetBGMAudioSource().volume = volume;
        }
        public float GetSFXSound_Volume() { return SFX_SOUND; }
        public void SetSFXSound_Volume(float volume)
        {
            SFX_SOUND = volume;
            JCS_SoundManager.instance.SetSoundVolume(JCS_SoundSettingType.SFX_SOUND, volume);
        }
        public void SetSkillsSound_Volume(float volume)
        {
            SKILLS_SOUND = volume;
            JCS_SoundManager.instance.SetSoundVolume(JCS_SoundSettingType.SKILLS_SOUND, volume);
        }
        public float GetSkillsSound_Volume()
        {
            return SKILLS_SOUND;
        }
        public float GetSoundBaseOnType(JCS_SoundSettingType type)
        {
            switch (type)
            {
                case JCS_SoundSettingType.BGM_SOUND: return GetBGM_Volume();
                case JCS_SoundSettingType.SFX_SOUND: return GetSFXSound_Volume();
                case JCS_SoundSettingType.SKILLS_SOUND: return GetSkillsSound_Volume();
            }

            JCS_Debug.LogError("JCS_GameSetting", "Get unknown volume...");

            return 0;
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = CheckSingleton(instance, this);
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

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_SoundSettings _old, JCS_SoundSettings _new)
        {
            _new.BGM_MUTE = _old.BGM_MUTE;
            _new.EFFECT_MUTE = _old.EFFECT_MUTE;
            _new.PERFONAL_EFFECT_MUTE = _old.PERFONAL_EFFECT_MUTE;
            _new.BGM_SOUND = _old.BGM_SOUND;
            _new.SFX_SOUND = _old.SFX_SOUND;
            _new.SKILLS_SOUND = _old.SKILLS_SOUND;
        }

        //----------------------
        // Private Functions

    }
}
