﻿/**
 * $File: JCS_FadeObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    public delegate void Fading(float alpha);

    /// <summary>
    /// Fade object to a particular alpha channel.
    /// </summary>
    public class JCS_FadeObject : JCS_UnityObject
    {
        /* Variables */

        public EmptyFunction onFadeOut = null;
        public EmptyFunction onFadeIn = null;

        public Fading onFading = null;

        private JCS_FadeType mFadeType = JCS_FadeType.FADE_IN;  // defaul as visible

        private float mAlpha = 1.0f;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_FadeObject)")]

        [Tooltip("Test Fade in/out with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [SerializeField]
        private KeyCode mFadeInKey = KeyCode.M;
        [SerializeField]
        private KeyCode mFadeOutKey = KeyCode.N;
#endif

        [Separator("Check Variables (JCS_FadeObject)")]

        [Tooltip("Is current fade object doing the effect? (fade in/out)")]
        [SerializeField]
        [ReadOnly]
        private bool mEffect = false;

        [Tooltip("Is current fade object visible?")]
        [SerializeField]
        [ReadOnly]
        private bool mVisible = true;

        [Separator("Runtime Variables (JCS_FadeObject)")]

        [Tooltip("How long it fades.")]
        [SerializeField]
        [Range(0.0f, 60.0f)]
        private float mFadeTime = 1.0f;

        [Tooltip("Override the action before it complete the action.")]
        [SerializeField]
        private bool mOverrideFade = false;

        [Tooltip("Maxinum of fade value.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mFadeInAmount = 1.0f;

        [Tooltip("Mininum of fade value.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mFadeOutAmount = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public bool Visible { get { return this.mVisible; } set { this.mVisible = value; } }
        public float FadeTime { get { return this.mFadeTime; } set { this.mFadeTime = value; } }
        public bool OverrideFade { get { return this.mOverrideFade; } set { this.mOverrideFade = value; } }
        public float Alpha { get { return this.mAlpha; } set { this.mAlpha = value; } }
        public float FadeInAmount { get { return this.mFadeInAmount; } set { this.mFadeInAmount = value; } }
        public float FadeOutAmount { get { return this.mFadeOutAmount; } set { this.mFadeOutAmount = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Start()
        {
            if (LocalColor.a <= 0.0f)
                mVisible = false;
            else
                mVisible = true;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif
            DoFade();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mFadeOutKey))
                FadeOut();
            if (Input.GetKeyDown(mFadeInKey))
                FadeIn();
        }
#endif

        /// <summary>
        /// Is the fade object fade in?
        /// </summary>
        /// <returns>
        /// true : is fade in.
        /// false : not fade in yet.
        /// </returns>
        public bool IsFadeIn()
        {
            return (this.mAlpha >= mFadeInAmount);
        }

        /// <summary>
        /// Is the fade object fade out?
        /// </summary>
        /// <returns>
        /// true : is fade out.
        /// false : not fade out yet.
        /// </returns>
        public bool IsFadeOut()
        {
            return (this.mAlpha <= mFadeOutAmount);
        }

        public void FadeOut() { FadeOut(mFadeTime); }
        public void FadeIn() { FadeIn(mFadeTime); }
        public void FadeOut(float time) { FadeEffect(JCS_FadeType.FADE_OUT, time); }
        public void FadeIn(float time) { this.FadeEffect(JCS_FadeType.FADE_IN, time); }

        /// <summary>
        /// Default function to point to, 
        /// prevent null reference exception.
        /// to save check null pointer performance.
        /// </summary>
        public static void DefaultFadeCallback()
        {
            // empty.
        }

        /// <summary>
        /// Do the fade effect.
        /// </summary>
        /// <param name="type"> Type to fade </param>
        /// <param name="time"> time to fade in/out, in seconds </param>
        private void FadeEffect(JCS_FadeType type, float time)
        {
            if (!mOverrideFade)
            {
                // Check is already fade out or fade in!
                if ((mVisible && type == JCS_FadeType.FADE_IN) ||
                    (!mVisible && type == JCS_FadeType.FADE_OUT))
                    return;
            }

            // enable the effect component
            this.LocalEnabled = true;

            switch (type)
            {
                case JCS_FadeType.FADE_OUT:
                    {
                        mAlpha = mFadeInAmount;
                        this.mVisible = false;
                    }
                    break;
                case JCS_FadeType.FADE_IN:
                    {
                        mAlpha = mFadeOutAmount;
                        this.mVisible = true;
                    }
                    break;
            }

            this.mFadeTime = time;
            this.mFadeType = type;
            this.mEffect = true;
        }

        /// <summary>
        /// Do the core fade effect.
        /// </summary>
        private void DoFade()
        {
            if (GetObjectType() == JCS_UnityObjectType.GAME_OBJECT &&
                JCS_GameManager.instance.GAME_PAUSE)
                return;

            if (!mEffect)
                return;

            switch (mFadeType)
            {
                case JCS_FadeType.FADE_OUT:
                    {
                        // Fade out effect complete
                        if (mAlpha < mFadeOutAmount)
                        {
                            this.LocalEnabled = false;

                            mEffect = false;

                            // do fade out callback
                            if (onFadeOut != null)
                                onFadeOut.Invoke();

                            return;
                        }

                        mAlpha -= JCS_Time.ItTime(mTimeType) / mFadeTime;
                    }
                    break;

                case JCS_FadeType.FADE_IN:
                    {
                        // Fade in effect complete
                        if (mAlpha > mFadeInAmount)
                        {
                            mEffect = false;

                            // do fade in callback
                            if (onFadeIn != null)
                                onFadeIn.Invoke();

                            return;
                        }

                        mAlpha += JCS_Time.ItTime(mTimeType) / mFadeTime;
                    }
                    break;
            }

            Color screenColor = this.LocalColor;
            screenColor.a = mAlpha;
            this.LocalColor = screenColor;

            if (onFading != null)
                onFading.Invoke(mAlpha);
        }
    }
}
