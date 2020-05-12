/**
 * $File: JCS_2DShakeEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Effect that shake the transform in 2D.
    /// </summary>
    public class JCS_2DShakeEffect
        : JCS_2DEffect
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DShakeEffect) **")]

        [Tooltip("Test with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Do wave effect key.")]
        [SerializeField]
        private KeyCode mDoShakeEffectKey = KeyCode.Y;
#endif

        [Header("** Runtime Variables (JCS_2DShakeEffect) **")]

        [Tooltip("Override the effect even the the effect is enabled already.")]
        [SerializeField]
        private bool mRepeatOverride = false;

        [Tooltip("How long it shakes.")]
        [SerializeField]
        [Range(0.001f, 360.0f)]
        private float mShakeTime = 1.0f;

        [Tooltip("How intense it shakes.")]
        [SerializeField]
        private float mShakeMargin = 3.0f;

        [Tooltip("Shake for each steps.")]
        [SerializeField]
        private float mShakeSteps = 5.0f;

        // Support
        private float mShakeTimer = 0.0f;
        private Vector3 mShakeOrigin = Vector3.zero;

        [Header("- Camera")]

        [Tooltip("(NOTE: If the effect object is camera, plz fill the camera in here.)")]
        [SerializeField]
        private JCS_2DCamera m2DCamera = null;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound played when effect occurs.")]
        [SerializeField]
        private AudioClip mShakeSound = null;

        /* Setter & Getter */

        public bool RepeatOverride { get { return this.mRepeatOverride; } set { this.mRepeatOverride = value; } }
        public float ShakeTime { get { return this.mShakeTime; } set { this.mShakeTime = value; } }
        public float ShakeMargin { get { return this.mShakeMargin; } set { this.mShakeMargin = value; } }
        public float ShakeSteps { get { return this.mShakeSteps; } set { this.mShakeSteps = value; } }

        public JCS_SoundPlayer SoundPlayer { get { return this.mSoundPlayer; } set { this.mSoundPlayer = value; } }
        public AudioClip ShakeSound { get { return this.mShakeSound; } set { this.mShakeSound = value; } }

        /* Functions */

        private void Awake()
        {
            if (m2DCamera == null)
                m2DCamera = this.GetComponent<JCS_2DCamera>();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoEffect();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKey(mDoShakeEffectKey))
            {
                DoShake();
            }
        }
#endif

        /// <summary>
        /// Do the shake with default shake time and shake margin.
        /// </summary>
        public void DoShake()
        {
            DoShake(mShakeTime, mShakeMargin);
        }

        /// <summary>
        /// Do the shake effect with time and margin.
        /// </summary>
        /// <param name="time"> time to do the shake. </param>
        /// <param name="margin"> margin to do the shake. </param>
        public void DoShake(float time, float margin)
        {
            if (!mRepeatOverride)
            {
                // if is doing the effect
                if (mEffect)
                {
                    // is effecting.
                    return;
                }
            }

            // only record down the first time
            if (!mEffect)
                this.mShakeOrigin = this.transform.position;

            this.mShakeTime = time;
            this.mShakeTimer = 0;
            this.mShakeMargin = margin;

            mEffect = true;

            // Disable the input.
            if (mStopInputWhileThisEffect)
                JCS_GameManager.instance.GAME_PAUSE = true;

            PlayeSound();
        }

        /// <summary>
        /// Do the actual shake job.
        /// </summary>
        private void DoEffect()
        {
            if (!mEffect)
                return;

            Vector3 pos = mShakeOrigin;

            if (m2DCamera != null)
            {
                this.mShakeOrigin.x = m2DCamera.GetTargetTransform().position.x;
                this.mShakeOrigin.y = m2DCamera.GetTargetTransform().position.y;
            }

            mShakeTimer += Time.deltaTime;

            if (mShakeTimer < mShakeTime)
            {
                // shake randomly
                // shakeTime / shakeTimer = shakeRate
                pos.x += (JCS_Random.Range(-1.0f, 1.0f + 1.0f)) * mShakeMargin * (mShakeTime / mShakeTimer) / mShakeSteps;
                pos.y += (JCS_Random.Range(-1.0f, 1.0f + 1.0f)) * mShakeMargin * (mShakeTime / mShakeTimer) / mShakeSteps;

                // apply pos
                this.transform.position = pos;
            }
            else
            {
                // back to original position
                this.transform.position = mShakeOrigin;

                mShakeTimer = 0.0f;
                mEffect = false;

                // Enable the input
                if (mStopInputWhileThisEffect)
                    JCS_GameManager.instance.GAME_PAUSE = false;
            }
        }

        /// <summary>
        /// Play shake sound.
        /// </summary>
        private void PlayeSound()
        {
            if (mShakeSound == null)
                return;

            if (mSoundPlayer)
            {
                mSoundPlayer.PlayOneShot(mShakeSound);
            }
            else
            {
                JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mShakeSound);
            }
        }
    }
}
