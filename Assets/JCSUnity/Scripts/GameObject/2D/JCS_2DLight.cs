/**
 * $File: JCS_2DLight.cs $
 * $Date: 2017-04-06 22:37:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 2D light like 'MapleStory ' v62 login screen.
    /// </summary>
    [RequireComponent(typeof(JCS_AlphaObject))]
    public class JCS_2DLight
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_AlphaObject mAlphaObject = null;


        [Header("** Runtime Variables (JCS_2DLight) **")]

        [Tooltip("Active this?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Time to change target light value.")]
        [SerializeField] [Range(0.1f, 10.0f)]
        private float mTimeToChange = 2.0f;

        [Tooltip("Time variable adjust the time to change variable.")]
        [SerializeField] [Range(0, 5)]
        private float mAdjustTimeToChange = 1.5f;

        // Real time to change
        private float mRealTimeToChange = 0;

        // use to check if the object faded?
        private bool mFaded = false;

        // timer to check to do the fade.
        private float mTimeToChangeTimer = 0;


        [Header("** Min/Max Settings (JCS_2DLight) **")]

        [Tooltip("Mininum value of the light can fade.")]
        [SerializeField] [Range(0, 1.0f)]
        private float mMinFadeValue = 0.0f;

        [Tooltip("Maxinum value of the light can fade.")]
        [SerializeField]
        [Range(0, 1.0f)]
        private float mMaxFadeValue = 1.0f;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mAlphaObject = this.GetComponent<JCS_AlphaObject>();
        }

        private void Update()
        {
            // check active.
            if (!mActive)
                return;

            DoFade();
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

        /// <summary>
        /// Do the fade algorithm.
        /// </summary>
        private void DoFade()
        {
            // if faded, redefine new time zone.
            if (mFaded)
                ResetTimeZone();

            mTimeToChangeTimer += Time.deltaTime;

            if (mTimeToChangeTimer < mRealTimeToChange)
                return;

            this.mAlphaObject.TargetAlpha = JCS_Random.Range(mMinFadeValue, mMaxFadeValue);

            // set flag for time zone.
            mFaded = true;

            // reset timer.
            mTimeToChangeTimer = 0;
        }

        /// <summary>
        /// Reset real time fade to change value base on 
        /// the setting.
        /// </summary>
        private void ResetTimeZone()
        {
            mRealTimeToChange = mTimeToChange + JCS_Random.Range(-mAdjustTimeToChange, mAdjustTimeToChange);

            // reset timer.
            mTimeToChangeTimer = 0;

            mFaded = false;
        }

    }
}
