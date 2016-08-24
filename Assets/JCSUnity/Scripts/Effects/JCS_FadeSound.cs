/**
 * $File: JCS_FadeSound.cs $
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
    public class JCS_FadeSound : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private AudioSource mAudioSource = null;

        private JCS_FadeType mType = JCS_FadeType.NONE;
        [SerializeField] private bool mEffect = false;

        [SerializeField] private float mFadeOutTime = 1.0f;
        [SerializeField] private float mFadeInTime = 1.0f;

        private float mTargetVolume = 0.0f;
        private float mRecordVolume = 0.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetFadeOutTime(float t) { this.mFadeOutTime = t; }
        public void SetFadeInTime(float t) { this.mFadeInTime = t; }

        //========================================
        //      Unity's function
        //------------------------------
        

        private void Update()
        {
            if (!mEffect)
                return;

            switch (mType)
            {
                case JCS_FadeType.FADE_OUT:
                    {
                        mAudioSource.volume -= mRecordVolume / mFadeOutTime * Time.deltaTime;

                        if (mAudioSource.volume <= mTargetVolume)
                        {
                            mEffect = false;
                            mAudioSource.volume = mTargetVolume;
                        }
                    }
                    break;
                case JCS_FadeType.FADE_IN:
                    {
                        mAudioSource.volume += mRecordVolume / mFadeInTime * Time.deltaTime;

                        if (mAudioSource.volume >= mTargetVolume)
                        {
                            mEffect = false;
                            mAudioSource.volume = mTargetVolume;
                        }
                    }
                    break;
            }

        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void SetAudioSource(AudioSource source)
        {
            if (source == null)
            {
                JCS_GameErrors.JcsErrors("JCS_FadeSound",   "source assign are null references...");
                return;
            }

            if (mAudioSource != null)
            {
                return;
            }

            this.mAudioSource = source;

            // record down the volume, 
            // so when is fade in this will become the 
            // target sound!
            mRecordVolume = mAudioSource.volume;
        }
        public void FadeOut(float target_volume, float time)
        {
            if (mType == JCS_FadeType.FADE_OUT)
            {
                JCS_GameErrors.JcsErrors("JCS_FadeSound",   "Already Fade out");
                return;
            }

            if (mAudioSource == null)
            {
                JCS_GameErrors.JcsErrors("JCS_FadeSound",   "Cannot do the fade out effect without the source...");
                return;
            }

            SetFadeOutTime(time);

            mTargetVolume = target_volume;
            mRecordVolume = mAudioSource.volume;

            mType = JCS_FadeType.FADE_OUT;
            mEffect = true;
        }
        public void FadeIn(float target_volume, float time)
        {
            if (mType == JCS_FadeType.FADE_IN)
            {
                JCS_GameErrors.JcsErrors("JCS_FadeSound",   "Already Fade in");
                return;
            }

            if (mAudioSource == null)
            {
                JCS_GameErrors.JcsErrors("JCS_FadeSound",   "Cannot do the fade in effect without the source...");
                return;
            }

            SetFadeInTime(time);

            mAudioSource.volume = 0;
            mTargetVolume = target_volume;
            mRecordVolume = target_volume;

            mType = JCS_FadeType.FADE_IN;
            mEffect = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
