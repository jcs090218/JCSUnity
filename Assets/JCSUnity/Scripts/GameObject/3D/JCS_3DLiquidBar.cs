/**
 * $File: JCS_3DLiquidBar.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

/* 
 * If you had purchase a copy of 'SpriteMask' and willing to use this
 * component, uncomment a line below. 
 * 
 * SpriteMask: https://assetstore.unity.com/packages/tools/sprite-management/sprite-mask-27642
 */
//#define SPRITE_MASK

using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// If finding for 2D Liquid bar to do the health bar effect,
    /// please use the "JCS_GUILiquidBar" instead.
    /// </summary>
    public class JCS_3DLiquidBar
        : JCS_LiquidBar
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_3DLiquidBar) **")]

        [Tooltip("Test functionalities works?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Make the bar to min value.")]
        [SerializeField]
        private KeyCode mLackKey = KeyCode.J;

        [Tooltip("Make the bar to max value.")]
        [SerializeField]
        private KeyCode mFullKey = KeyCode.K;

        [Tooltip("Make the bar to half value.")]
        [SerializeField]
        private KeyCode mHalfKey = KeyCode.H;

        [Tooltip("Fixed the value if the min and max value has changed.")]
        [SerializeField]
        private KeyCode mFixedKey = KeyCode.X;
#endif


        [Header("** Check Variables (JCS_3DLiquidBar) **")]

        [SerializeField]
        private Vector3 mMaskTargetPosition = Vector3.zero;


#if (SPRITE_MASK)
        [Header("** Initilaize Variables (JCS_3DLiquidBar) **")]

        [Tooltip("Sprite mask that mask out the inner bar sprite.")]
        [SerializeField]
        private SpriteMask mSpriteMask = null;
#endif

        [Tooltip("Please put the under texture bar here.")]
        [SerializeField]
        private SpriteRenderer mBarSpriteRenderer = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        protected void Start()
        {
            GetContainerData();
        }

        protected void LateUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            // check all components needed are
            // avaliable.
            if (mBarSpriteRenderer == null)
                return;

            // update value.
            UpdateInfo();

            // do recover
            DoRecover();

            // do gui movement
            TowardToTargetValue();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mLackKey))
                Lack();
            if (JCS_Input.GetKeyDown(mFullKey))
                Full();

            // half
            if (JCS_Input.GetKeyDown(mHalfKey))
                SetCurrentValue(MaxValue / 2);

            if (JCS_Input.GetKeyDown(mFixedKey))
                FixPercentage();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Attach 2d live object so it will follow the
        /// value from this object.
        /// </summary>
        /// <param name="obj"> Info for liquid bar to follow. </param>
        public override void AttachInfo(JCS_LiquidBarInfo info)
        {
            this.mInfo = info;

            UpdateInfo();
        }

        /// <summary>
        /// Set the maxinum value of this container.
        /// </summary>
        /// <param name="val"> value to max </param>
        public override void SetMaxValue(float val)
        {
            if (val <= mMinValue)
            {
                JCS_Debug.LogError(
                    "Max value u r setting cannot be lower than min value.");

                return;
            }

            this.mMaxValue = val;

            if (mInfo != null)
                mInfo.MaxValue = (int)val;

            FixPercentage();
        }

        /// <summary>
        /// Set the mininum value of this container.
        /// </summary>
        /// <param name="val"> value to min </param>
        public override void SetMinValue(float val)
        {
            if (val >= mMaxValue)
            {
                JCS_Debug.LogError(
                    "Min value u r setting cannot be higher than max value.");

                return;
            }

            this.mMinValue = val;

            if (mInfo != null)
                mInfo.MinValue = (int)val;

            FixPercentage();
        }

        /// <summary>
        /// Set the value directly.
        /// </summary>
        /// <param name="val"> value to set </param>
        public override void SetCurrentValue(float val)
        {
            if (!mOverrideZero)
            {
                if (mZeroed)
                {
                    mCurrentValue = 0;
                    return;
                }
            }

            this.mCurrentValue = val;

            // cannot lower than min container value
            if (mCurrentValue < mMinValue)
                mCurrentValue = mMinValue;

            // cannot higher than max container value
            if (mCurrentValue > mMaxValue)
                mCurrentValue = mMaxValue;

            if (mInfo != null)
                mInfo.CurrentValue = (int)mCurrentValue;

            FixPercentage();

            // do call back
            DoZeroCallback();
        }

        /// <summary>
        /// Delta the current value.
        /// </summary>
        /// <param name="deltaVal"> delta to current value's value </param>
        public override void DeltaCurrentValue(float deltaVal)
        {
            SetCurrentValue(mCurrentValue + deltaVal);
        }

        /// <summary>
        /// Delta to current value in absolute positive value.
        /// </summary>
        public override void DeltaAdd(float val)
        {
            DeltaCurrentValue(JCS_Mathf.ToPositive(val));
        }

        /// <summary>
        /// Delta to current value in absolute negative value.
        /// </summary>
        public override void DeltaSub(float val)
        {
            DeltaCurrentValue(JCS_Mathf.ToNegative(val));
        }

        /// <summary>
        /// Full the liquid bar.
        /// </summary>
        public override void Full()
        {
            SetCurrentValue(mMaxValue);
        }

        /// <summary>
        /// Zero the liquid bar.
        /// </summary>
        public override void Lack()
        {
            SetCurrentValue(mMinValue);
        }

        /// <summary>
        /// Check if the value are able to cast.
        /// Mana value must higher than the cast value.
        /// </summary>
        /// <param name="val"> value to cast </param>
        /// <returns> true: able to cast, 
        ///           false: not able to cast </returns>
        public override bool IsAbleToCast(float val)
        {
            // able to cast the spell
            if (GetCurrentValue() >= JCS_Mathf.ToPositive(val))
                return true;

            // not able to cast the spell
            return false;
        }

        /// <summary>
        /// Check if able to cast the spell, 
        /// if true cast it.
        /// </summary>
        /// <param name="val"> value to cast </param>
        public override bool IsAbleToCastCast(float val)
        {
            // able to cast the spell, cast it.
            if (IsAbleToCast(val))
            {
                // cost the mana, and ready to cast the spell
                DeltaCurrentValue(val);
                return true;
            }

            // not enough value, cannot cast the spell...
            return false;
        }

        /// <summary>
        /// Current value in liquid bar.
        /// </summary>
        /// <returns> value in liquid bar </returns>
        public override float GetCurrentValue()
        {
            return this.mCurrentValue;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Get the container data so it could so the correct 
        /// position check and value check.
        /// </summary>
        private void GetContainerData()
        {
            if (mBarSpriteRenderer == null)
                return;

#if (SPRITE_MASK)
            // find the width and height of the image from sprite renderer
            Vector2 maskSize = mSpriteMask.size;

            switch (GetAlign())
            {
                case JCS_Align.ALIGN_LEFT:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.x;
                        mMinPos = mMaxPos - (maskSize.x * mSpriteMask.transform.localScale.x);
                    }
                    break;

                case JCS_Align.ALIGN_RIGHT:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.x;
                        mMinPos = mMaxPos + (maskSize.x * mSpriteMask.transform.localScale.x);
                    }
                    break;

                case JCS_Align.ALIGN_TOP:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.y;
                        mMinPos = mMaxPos + (maskSize.y * mSpriteMask.transform.localScale.y);
                    }
                    break;

                case JCS_Align.ALIGN_BOTTOM:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.y;
                        mMinPos = mMaxPos - (maskSize.y * mSpriteMask.transform.localScale.y);
                    }
                    break;
            }
#endif

            // do starting percent
            FixPercentage();
        }

        /// <summary>
        /// Fix the percentage of the liquid bar shown.
        /// </summary>
        private void FixPercentage()
        {
            if (mCurrentValue < mMinValue ||
                mCurrentValue > mMaxValue)

            {
                //JCS_Debug.LogWarning(
                //    "Value should with in min(" + mMinValue + ") ~ max(" + mMaxValue + ") value");

                return;
            }

            float realValue = mMaxValue - mMinValue;
            float currentPercentage = mCurrentValue / realValue;

            float realDistance = (mMaxPos - mMinPos) * currentPercentage;

            switch (GetAlign())
            {
                case JCS_Align.ALIGN_LEFT:
                case JCS_Align.ALIGN_RIGHT:
                    mMaskTargetPosition.x = mMinPos + realDistance;
                    break;
                case JCS_Align.ALIGN_BOTTOM:
                case JCS_Align.ALIGN_TOP:
                    mMaskTargetPosition.y = mMinPos + realDistance;
                    break;
            }
        }

        /// <summary>
        /// Movement of the liquid bar.
        /// </summary>
        private void TowardToTargetValue()
        {
#if (SPRITE_MASK)
            Vector3 speed = (mMaskTargetPosition - mSpriteMask.transform.localPosition) / mDeltaFriction * Time.deltaTime;
            mSpriteMask.transform.localPosition += speed;
#endif
        }

        /// <summary>
        /// Do the recover timer and add per timer is reached.
        /// </summary>
        private void DoRecover()
        {
            if (!mRecoverEffect)
                return;

            mRecoverTimer += Time.deltaTime;

            if (mRecoverTimer < mTimeToRecover)
                return;

            // do recover
            DeltaCurrentValue(mRecoverValue);

            // reset timer.
            mRecoverTimer = 0;
        }

        /// <summary>
        /// Do call back if call back was there.
        /// </summary>
        private void DoZeroCallback()
        {
            if (mCurrentValue != 0)
                return;

            // do zero call back.
            if (ZeroCallbackFunc != null)
            {
                ZeroCallbackFunc.Invoke();
            }

            // did zero.
            mZeroed = true;
        }

        /// <summary>
        /// Follow the value info attached.
        /// </summary>
        private void UpdateInfo()
        {
            // without info attach, we cannot
            // do anything.
            if (mInfo == null)
                return;

            SetMinValue(mInfo.MinValue);
            SetMaxValue(mInfo.MaxValue);

            SetCurrentValue(mInfo.CurrentValue);
        }

    }
}
