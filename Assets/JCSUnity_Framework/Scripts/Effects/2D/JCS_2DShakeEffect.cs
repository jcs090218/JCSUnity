/**
 * $File: JCS_2DShakeEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    public class JCS_2DShakeEffect 
        : JCS_2DEffect
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables **")]
        [Tooltip("Override the effect even the the effect is enabled already.")]
        [SerializeField] private bool mRepeatOverride = false;
        [Tooltip("How long it shake.")]
        [SerializeField] private float mShakeTime = 1;
        [Tooltip("How intense it shake.")]
        [SerializeField] private float mShakeMargin = 3;

        // Support
        private float mShakeTimer = 0;
        private Vector3 mShakeOrigin = Vector3.zero;

        [Header("NOTE: If the effect object is camera, plz fill the camera in here.")]
        [SerializeField] private JCS_2DCamera mJCS_2DCamera = null;


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

        private void Test()
        {
            if (JCS_Input.GetKey(KeyCode.Y))
            {
                DoShake();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DoShake()
        {
            DoShake(mShakeTime, mShakeMargin);
        }
        public void DoShake(float time, float margin)
        {
            if (!mRepeatOverride)
            {
                // if is doing the effect
                if (mEffect)
                {
                    JCS_GameErrors.JcsErrors("JCS_2DShakeEffect");
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

            // Dis-enable the input
            if (mStopInputWhileThisEffect)
                JCS_GameManager.instance.GAME_PAUSE = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
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
                pos.x += (JCS_UsefualFunctions.JCS_FloatRange(-1, 1)) * mShakeMargin * (mShakeTime / mShakeTimer) / 5;
                pos.y += (JCS_UsefualFunctions.JCS_FloatRange(-1, 1)) * mShakeMargin * (mShakeTime / mShakeTimer) / 5;

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
