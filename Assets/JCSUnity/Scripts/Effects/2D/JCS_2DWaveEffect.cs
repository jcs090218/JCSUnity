/**
 * $File: JCS_2DWaveEffect.cs $
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

    public class JCS_2DWaveEffect
        : JCS_2DEffect
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        private float mTime = 0;
        [Tooltip("height offset.")]
        [SerializeField] private float mWaveRestPosition = 0.0f;
        [Tooltip("How intense the wave is.")]
        [SerializeField] private float mAmplitude = 1.0f;
        [Tooltip("How fast per period in wave.")]
        [SerializeField] private float mFrequency = 2;
        private Vector3 mOrigin = Vector3.zero;
        [Tooltip("Effect Axis(Effect will do only on axis!, for random plz use the 3D version!)")]
        [SerializeField] private JCS_Axis mEffectAxis = JCS_Axis.AXIS_X;

        private float mEffectTime = 0;
        private float mEffectPeriod = 0;

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
            if (JCS_Input.GetKeyDown(KeyCode.T))
            {
                DoWaveEffect(1, mFrequency);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DoWaveEffect(float time, float frequency)
        {
            // if is doing the effect
            if (mEffect)
                return;

            mTime = 0;
            mEffectTime = time;
            mFrequency = frequency;

            mOrigin = this.transform.position;

            // cps = cycle per second = wave speed

            mEffect = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void DoEffect()
        {
            if (!mEffect)
                return;

            if (mJCS_2DCamera != null)
            {
                this.mOrigin.x = mJCS_2DCamera.GetTargetTransform().position.x;
                this.mOrigin.y = mJCS_2DCamera.GetTargetTransform().position.y;
            }

            mTime += Time.deltaTime;

            if (mEffectTime > mTime)
            {
                Vector3 newPos = mOrigin;


                float force =
                    (mAmplitude *
                    Mathf.Cos(mTime * Mathf.PI * mFrequency * 2));

                switch (mEffectAxis)
                {
                    case JCS_Axis.AXIS_X:
                        newPos.x = force + 
                            (mWaveRestPosition + mOrigin.x);
                        break;
                    case JCS_Axis.AXIS_Y:
                        newPos.y = force + 
                            (mWaveRestPosition + mOrigin.y);
                        break;
                    case JCS_Axis.AXIS_Z:
                        newPos.z = force +
                            (mWaveRestPosition + mOrigin.z);
                        break;
                }
                

                this.transform.position = newPos;
            }
            else
            {
                this.transform.position = mOrigin;

                mEffect = false;
            }
        }

    }
}
