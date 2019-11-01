/**
 * $File: JCS_SoundPlayer.cs $
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
    /// Sound Player for any object that need to player 
    /// sound effect.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_SoundPlayer 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private AudioSource mAudioSource = null;

        [Tooltip("Sound setting type for this perfab.")]
        [SerializeField]
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public AudioSource GetAudioSource() { return this.mAudioSource; }
        public JCS_SoundSettingType GetSoundSettingType() { return this.mSoundSettingType; }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            mAudioSource = this.GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            // let sound manager know we are here and ready
            // to be manage be him(Sound Manager)
            JCS_SoundManager.instance.AssignSoundSource(mSoundSettingType, GetAudioSource());
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Play one shot of sound.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="type"></param>
        public void PlayOneShot(AudioClip clip, JCS_SoundSettingType type)
        {
            float volume = 0;
            switch (type)
            {
                case JCS_SoundSettingType.BGM_SOUND:
                    volume = JCS_SoundSettings.instance.GetBGM_Volume();
                    break;
                case JCS_SoundSettingType.SFX_SOUND:
                    volume = JCS_SoundSettings.instance.GetSFXSound_Volume();
                    break;
                case JCS_SoundSettingType.SKILLS_SOUND:
                    volume = JCS_SoundSettings.instance.GetSkillsSound_Volume();
                    break;
            }

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Play one shot of sound.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        public void PlayOneShot(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null)
                return;

            GetAudioSource().PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Play one shot of sound.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        /// <param name="type"></param>
        public void PlayOneShot(AudioClip clip, float volume, JCS_SoundType type)
        {
            SetSoundType(type);

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Play one shot of sound.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="type"></param>
        public void PlayOneShot(AudioClip clip, JCS_SoundType type)
        {
            SetSoundType(type);

            GetAudioSource().PlayOneShot(clip);
        }

        /// <summary>
        /// play sound when is not playing any sound.
        /// </summary>
        public void PlayOneShotWhileNotPlaying(AudioClip clip, float volume = 1.0f)
        {
            if (GetAudioSource() == null)
                return;

            if (GetAudioSource().isPlaying)
                return;

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Stop all the sound then play the this sound.
        /// </summary>
        public void PlayOneShotInterrupt(AudioClip clip, float volume = 1.0f)
        {
            if (GetAudioSource() == null)
                return;

            GetAudioSource().Stop();

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Play sound by method.
        /// </summary>
        /// <param name="clip"> clip to play. </param>
        /// <param name="method"> method of play. </param>
        /// <param name="volume"> volume to play. </param>
        public void PlayOneShotByMethod(AudioClip clip, JCS_SoundMethod method, float volume = 1.0f)
        {
            switch (method)
            {
                case JCS_SoundMethod.PLAY_SOUND:
                    PlayOneShot(clip, volume);
                    break;
                case JCS_SoundMethod.PLAY_SOUND_WHILE_NOT_PLAYING:
                    PlayOneShotWhileNotPlaying(clip, volume);
                    break;
                case JCS_SoundMethod.PLAY_SOUND_INTERRUPT:
                    PlayOneShotInterrupt(clip, volume);
                    break;
            }
        }

        //----------------------
        // Protected Functions

        /// <summary>
        /// Set either the sound player in 2d space or 3d space.
        /// </summary>
        /// <param name="type"> 2d or 3d space. </param>
        protected void SetSoundType(JCS_SoundType type)
        {
            switch (type)
            {
                case JCS_SoundType.SOUND_2D:
                    GetAudioSource().spatialBlend = 0;
                    break;
                case JCS_SoundType.SOUND_3D:
                    GetAudioSource().spatialBlend = 1;
                    break;
            }
        }

        //----------------------
        // Private Functions

    }
}
