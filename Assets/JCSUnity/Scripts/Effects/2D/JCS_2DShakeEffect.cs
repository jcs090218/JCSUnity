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
    /// Effect that shake the gameobject.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_2DShakeEffect 
        : JCS_2DEffect
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_SoundPlayer mSoundPlayer = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DWaveEffect) **")]

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
        [SerializeField] [Range(0.001f, 360.0f)]
        private float mShakeTime = 1.0f;

        [Tooltip("How intense it shakes.")]
        [SerializeField]
        private float mShakeMargin = 3.0f;

        // Support
        private float mShakeTimer = 0;
        private Vector3 mShakeOrigin = Vector3.zero;

        [Header("NOTE: If the effect object is camera, plz fill the camera in here.")]
        [SerializeField]
        private JCS_2DCamera mJCS_2DCamera = null;

        [Header("** Sound Settings (JCS_2DShakeEffect) **")]

        [Tooltip("Sound played when effect occurs.")]
        [SerializeField]
        private AudioClip mShakeSound = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool RepeatOverride { get { return this.mRepeatOverride; } set { this.mRepeatOverride = value; } }
        public float ShakeTime { get { return this.mShakeTime; } set { this.mShakeTime = value; } }
        public float ShakeMargin { get { return this.mShakeMargin; } }

        public AudioClip ShakeSound { get { return this.mShakeSound; } set { this.mShakeSound = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            if (mJCS_2DCamera == null)
                mJCS_2DCamera = this.GetComponent<JCS_2DCamera>();
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

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

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

            // play sound effect!
            mSoundPlayer.PlayOneShot(mShakeSound);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do the actual shake job.
        /// </summary>
        private void DoEffect()
        {
            if (!mEffect)
                return;

            Vector3 pos = mShakeOrigin;

            if (mJCS_2DCamera != null)
            {
                this.mShakeOrigin.x = mJCS_2DCamera.GetTargetTransform().position.x;
                this.mShakeOrigin.y = mJCS_2DCamera.GetTargetTransform().position.y;
            }

            mShakeTimer += Time.deltaTime;

            if (mShakeTimer < mShakeTime)
            {
                // shake randomly
                // shakeTime / shakeTimer = shakeRate
                pos.x += (JCS_Random.Range(-1, 1 + 1)) * mShakeMargin * (mShakeTime / mShakeTimer) / 5;
                pos.y += (JCS_Random.Range(-1, 1 + 1)) * mShakeMargin * (mShakeTime / mShakeTimer) / 5;

                // apply pos
                this.transform.position = pos;
            }
            else
            {
                // back to original position
                this.transform.position = mShakeOrigin;

                mShakeTimer = 0;
                mEffect = false;

                // Enable the input
                if (mStopInputWhileThisEffect)
                    JCS_GameManager.instance.GAME_PAUSE = false;
            }
        }

    }
}
