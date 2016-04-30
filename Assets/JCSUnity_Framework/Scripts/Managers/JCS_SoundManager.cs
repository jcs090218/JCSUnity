/**
 * $File: JCS_SoundManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_SoundManager : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_SoundManager instance = null;

        //----------------------
        // Private Variables
        private AudioListener mAudioListener = null;


        [SerializeField] private AudioSource mBGM = null;

        // environment, ui, etc.
        private JCS_Vector<AudioSource> mSFXSounds = null;

        // personal stuff, personal skill, personal jump walk, etc.
        private JCS_Vector<AudioSource> mSkillsSounds = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetAudioListener(AudioListener al) { this.mAudioListener = al; }
        public AudioListener GetAudioListener() { return this.mAudioListener; }
        public AudioSource GetBackgroundMusic() { return this.mBGM; }
        public void SetBackgroundMusic(AudioSource music)
        {
            this.mBGM = music;

            this.mBGM.volume = JCS_GameSettings.GetBGM_Volume();
            this.mBGM.mute = JCS_GameSettings.BGM_MUTE;
        }
        public JCS_Vector<AudioSource> GetEffectSounds() { return this.mSFXSounds; }

        //========================================
        //      Unity's function
        //------------------------------
        private void OnApplicationFocus(bool focusStatus)
        {
            if (GetAudioListener() == null)
                return;

            // turn off all the sound effect
            if (!focusStatus)
            {
                GetAudioListener().enabled = false;
            }
            else
            {
                
                GetAudioListener().enabled = true;
            }
        }
        private void Awake()
        {
            instance = this;

            mSFXSounds = new JCS_Vector<AudioSource>();
            mSkillsSounds = new JCS_Vector<AudioSource>();
        }

        private void Start()
        {
            JCS_Camera gm = (JCS_2DCamera)JCS_GameManager.instance.GetJCSCamera();
            if (gm == null)
            {
                JCS_GameErrors.JcsErrors("JCS_SoundManager", -1, "There is no \"JCS_Camera\" assign!");
                return;
            }

            // Reset the sound every scene
            SetSFXSoundVolume(JCS_GameSettings.GetSFXSound_Volume());
            SetSkillsSoundVolume(JCS_GameSettings.GetSkillsSound_Volume());
            SetSFXSoundMute(JCS_GameSettings.EFFECT_MUTE);
            SetSkillsSoundMute(JCS_GameSettings.PERFONAL_EFFECT_MUTE);
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        /// <summary>
        /// Push to the sound effect into array ready for use!
        /// </summary>
        /// <param name="sound"></param>
        
        public void PlayOneShotSFXSound(int index)
        {
            AudioSource aud = mSFXSounds.at(index);
            if (aud.clip != null)
                aud.PlayOneShot(aud.clip, JCS_GameSettings.GetSFXSound_Volume());
        }
        
        
        public void AssignSoundSource(JCS_SoundSettingType type, AudioSource source)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE:
                    return;
                case JCS_SoundSettingType.BGM_SOUND:
                    SetBackgroundMusic(source);
                    break;
                case JCS_SoundSettingType.SFX_SOUND:
                    AssignSFX_Sound(source);
                    break;
                case JCS_SoundSettingType.SKILLS_SOUND:
                    AssignSkillsSound(source);
                    break;
            }
        }
        public void SetSoundVolume(JCS_SoundSettingType type, float volume)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE:
                    return;
                case JCS_SoundSettingType.BGM_SOUND:
                    GetBackgroundMusic().volume = volume;
                    break;
                case JCS_SoundSettingType.SFX_SOUND:
                    SetSFXSoundVolume(volume);
                    break;
                case JCS_SoundSettingType.SKILLS_SOUND:
                    SetSkillsSoundVolume(volume);
                    break;
            }
        }
        public void SetSoundMute(JCS_SoundSettingType type, bool act)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE:
                    return;
                case JCS_SoundSettingType.BGM_SOUND:
                    GetBackgroundMusic().mute = act;
                    break;
                case JCS_SoundSettingType.SFX_SOUND:
                    SetSFXSoundMute(act);
                    break;
                case JCS_SoundSettingType.SKILLS_SOUND:
                    SetSkillsSoundMute(act);
                    break;
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void SetSoundVolume(JCS_Vector<AudioSource> list, float vol)
        {
            for (int index = 0;
                index < list.length;
                ++index)
            {
                list.at(index).volume = vol;
            }
        }
        private void SetSoundtMute(JCS_Vector<AudioSource> list, bool act)
        {
            for (int index = 0;
                index < list.length;
                ++index)
            {
                list.at(index).mute = act;
            }
        }

        private void AssignSFX_Sound(AudioSource sound)
        {
            AssignSoundToList(mSFXSounds, sound);
        }
        private void AssignSkillsSound(AudioSource sound)
        {
            AssignSoundToList(mSkillsSounds, sound);
        }
        private void AssignSoundToList(JCS_Vector<AudioSource> list, AudioSource sound)
        {
            if (sound == null)
            {
                JCS_GameErrors.JcsErrors("JCS_SoundManager", -1, "Assigning Source that is null...");
                return;
            }

            list.push(sound);
            sound.volume = JCS_GameSettings.GetSFXSound_Volume();
        }
        private void SetSFXSoundVolume(float vol)
        {
            SetSoundVolume(mSFXSounds, vol);
        }
        private void SetSkillsSoundVolume(float vol)
        {
            SetSoundVolume(mSkillsSounds, vol);
        }
        private void SetSFXSoundMute(bool act)
        {
            SetSoundtMute(mSFXSounds, act);
        }
        private void SetSkillsSoundMute(bool act)
        {
            SetSoundtMute(mSkillsSounds, act);
        }

    }
}
