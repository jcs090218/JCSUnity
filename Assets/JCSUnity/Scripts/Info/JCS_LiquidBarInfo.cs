/**
 * $File: JCS_LiquidBarInfo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    /// <summary>
    /// Info to plugin to liquid bar, 
    /// to sync to information between the value and
    /// the gui part.
    /// </summary>
    public class JCS_LiquidBarInfo
        : MonoBehaviour
    {
        [SerializeField]
        protected string mTagName = "";

        [Tooltip("current value in liquid bar.")]
        [SerializeField] [Range(0, 999999999)]
        protected int mCurrentValue = 50;

        [Tooltip("min value in liquid bar.")]
        [SerializeField] [Range(0, 999999999)]
        protected int mMinValue = 0;

        [Tooltip("max value in liquid bar.")]
        [SerializeField] [Range(0, 999999999)]
        protected int mMaxValue = 100;

        //========================================
        //      setter / getter
        //------------------------------
        public string TagName { get { return this.mTagName; } set { this.mTagName = value; } }
        public int CurrentValue {
            get { return this.mCurrentValue; }
            set
            {
                this.mCurrentValue = value;

                // cannot lower than min value
                if (mCurrentValue < mMinValue)
                    mCurrentValue = mMinValue;

                // cannot higher than max value
                if (mCurrentValue >= mMaxValue)
                    mCurrentValue = mMaxValue;
            }
        }
        public int MinValue { get { return this.mMinValue; } set { this.mMinValue = value; } }
        public int MaxValue { get { return this.mMaxValue; } set { this.mMaxValue = value; } }

        //========================================
        //      Unity's function
        //------------------------------


        //========================================
        //      Self-Define
        //------------------------------



    }
}
