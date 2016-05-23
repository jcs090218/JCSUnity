/**
 * $File: JCS_LiquidBar.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    /// <summary>
    /// GUI liquid bar like health bar, mana bar etc.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_LiquidBar
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;
        private RectTransform mMaskRectTransform = null;

        [SerializeField] private Vector3 mMaskTargetPosition = Vector3.zero;

        [Header("** Initialize Variables **")]
        [Tooltip("please set this ")]
        [SerializeField] private Mask mMask;

        [Header("** Runtime Variables **")]
        [SerializeField] private float mDeltaFriction = 0.2f;
        [SerializeField] private float mMinValue = 0;
        [SerializeField] private float mMaxValue = 100;
        [SerializeField] private float mCurrentValue = 50;

        private float mMinPos = 0;
        private float mMaxPos = 0;

        // TODO(JenChieh): if i have time.
        [Tooltip("Health Direction")]
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        // TODO(JenChieh): Somewhat this work, better if i get the logic right
        //              then this can be optimize
        private uint mCountToGetContainerData = 0;

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
            // record down the position
            mRectTransform = this.GetComponent<RectTransform>();

            if (mMask == null)
            {
                JCS_GameErrors.JcsErrors("JCS_LiquidBar", -1, "");
                return;
            }

            this.transform.SetParent(mMask.transform);
            mMaskRectTransform = this.mMask.GetComponent<RectTransform>();

            mMaskTargetPosition = this.mMaskRectTransform.localPosition;

            // min value cannot be lower or equal to max value
            if (mMinValue >= mMaxValue)
                mMinValue = mMaxValue + 1;  // force min < max value
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            if (mMask == null)
                return;

            GetContainerData();

            TowardToTargetValue();
        }

        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.J))
                Lack();
            if (JCS_Input.GetKeyDown(KeyCode.K))
                Full();
            if (JCS_Input.GetKeyDown(KeyCode.X))
                FixPercentage();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void SetCurrentValue(float val)
        {
            this.mCurrentValue = val;
            FixPercentage();
        }
        public void Full()
        {
            SetCurrentValue(mMaxValue);
        }
        public void Lack()
        {
            SetCurrentValue(mMinValue);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void GetContainerData()
        {
            if (mCountToGetContainerData == 2)
                return;

            this.mRectTransform.SetParent(mMaskRectTransform.parent);

            mMaskTargetPosition = this.mRectTransform.localPosition;

            mMaxPos = mRectTransform.localPosition.x;
            mMinPos = mMaxPos - (mMaskRectTransform.sizeDelta.x * mMaskRectTransform.localScale.x);

            this.mRectTransform.SetParent(mMaskRectTransform);

            // starting percent
            FixPercentage();

            ++mCountToGetContainerData;
        }
        private void FixPercentage()
        {
            if (mCurrentValue < mMinValue || 
                mCurrentValue > mMaxValue)

            {
                JCS_GameErrors.JcsErrors("JCS_LiquidBar", -1, "Value should with in min(" + mMinValue + ") ~ max(" + mMaxValue + ") value");
                return;
            }

            float realValue = mMaxValue - mMinValue;
            float currentPercentage = mCurrentValue / realValue;


            float realDistance = (mMaxPos - mMinPos) * currentPercentage;
            mMaskTargetPosition.x = mMinPos + realDistance;
        }
        private void TowardToTargetValue()
        {
            Vector3 speed = (mMaskTargetPosition - mMaskRectTransform.localPosition) / mDeltaFriction * Time.deltaTime;
            mMaskRectTransform.localPosition += speed;
            mRectTransform.localPosition -= speed;
        }

    }
}
