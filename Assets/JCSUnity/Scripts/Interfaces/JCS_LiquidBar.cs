/**
 * $File: JCS_LiquidBar.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{
    // when value is zero do this call back
    public delegate void ZeroCallback();

    /// <summary>
    /// 
    /// </summary>
    public abstract class JCS_LiquidBar
        : MonoBehaviour
    {
        //========================================
        //      Variable-Define
        //------------------------------

        private const float MIN_LIQUID_BAR_VALUE = -9999999;
        private const float MAX_LIQUID_BAR_VALUE = 9999999;

        // when value is zero do this call back
        protected ZeroCallback mZeroCallbackFunc = null;


        [Header("** Check Variables (JCS_LiquidBar) **")]

        [SerializeField]
        protected JCS_LiquidBarInfo mInfo = null;

        [Header("** Initilaize Variables (JCS_LiquidBar) **")]

        [Tooltip("Once it set to zero, but still override this.")]
        [SerializeField]
        protected bool mOverrideZero = false;

        // check if object do zero.
        protected bool mZeroed = false;


        [Header("** Initialize Variables (JCS_LiquidBar) **")]

        [Tooltip("Align on which side? (top/bottom/right/left)")]
        [SerializeField]
        protected JCS_Align mAlign = JCS_Align.ALIGN_LEFT;


        [Header("** Runtime Variables (JCS_LiquidBar) **")]

        [Tooltip(@"How fast the liquid bar move approach to 
target position/value.")]
        [SerializeField] [Range(0.01f, 10.0f)]
        protected float mDeltaFriction = 0.2f;

        [Tooltip("Mininum value of the liquid bar.")]
        [SerializeField]
        [Range(MIN_LIQUID_BAR_VALUE, MAX_LIQUID_BAR_VALUE)]
        protected float mMinValue = 0;

        [Tooltip("Maxinum value of the liquid bar.")]
        [SerializeField]
        [Range(MIN_LIQUID_BAR_VALUE, MAX_LIQUID_BAR_VALUE)]
        protected float mMaxValue = 100;

        [Tooltip("")]
        [SerializeField]
        protected float mCurrentValue = 50;

        protected float mMinPos = 0;
        protected float mMaxPos = 0;


        [Header("** Optional Variables (JCS_LiquidBar) **")]

        [Tooltip("Information Image set here.")]
        [SerializeField]
        protected Image mInfoImage = null;


        [Header("** Asmptotic Recover Effect (JCS_LiquidBar) **")]

        [Tooltip("Enable the recover effect?")]
        [SerializeField]
        protected bool mRecoverEffect = true;

        [Tooltip(@"Time for one recover. If the 
time is minimal will recover like per frame.")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        protected float mTimeToRecover = 1.0f;

        // timer to do one recover.
        protected float mRecoverTimer = 0;

        [Tooltip(@"Recover Value per time. Careful that 
recover can be damage too.")]
        [SerializeField]
        [Range(-300000.0f, 300000.0f)]
        protected float mRecoverValue = 1;

        [Header("- addition settings")]
        [Tooltip("Will try to go back to the original value.")]
        [SerializeField]
        protected bool mBackToRecordRecoverValue = true;

        // record down the value in order to get 
        // back the original value.
        protected float mRecordValue = 0;

        [SerializeField] [Range(0.01f, 7.0f)]
        protected float mGetBackFriction = 1.0f;


        //========================================
        //      setter / getter
        //------------------------------
        public ZeroCallback ZeroCallbackFunc { get { return this.mZeroCallbackFunc; } set { this.mZeroCallbackFunc = value; } }

        public JCS_LiquidBarInfo Info { get { return this.mInfo; } }
        public Image InfoImage { get { return this.mInfoImage; } }

        public bool RecoverEffect { get { return this.mRecoverEffect; } set { this.mRecoverEffect = value; } }
        public float TimeToRecover { get { return this.mTimeToRecover; } set { this.mTimeToRecover = value; } }
        public float RecoverValue { get { return this.mRecoverValue; } set { this.mRecoverValue = value; } }

        public float MinValue { get { return this.mMinValue; } }
        public float MaxValue { get { return this.mMaxValue; } }

        public JCS_Align GetAlign() { return this.mAlign; }

        //========================================
        //      Unity's function
        //------------------------------

        protected virtual void Awake()
        { 
            // record down the recover value.
            this.mRecordValue = this.mRecoverValue;

            // get the image 
            if (mInfoImage == null)
                this.mInfoImage = this.GetComponent<Image>();
        }

        protected virtual void Update()
        {
            // 1) if the recover effect is off no need to 
            // get back the recorded recover value. 
            // 2) check the effect is on/off.
            if (!mRecoverEffect || !mBackToRecordRecoverValue)
                return;

            // Try to get back the original 
            // value (which we record down in 
            // the Awake function).
            this.mRecoverValue += (mRecordValue - mRecoverValue) / mGetBackFriction * Time.deltaTime;
        }

        //========================================
        //      Interface-Define
        //------------------------------

        /// <summary>
        /// Attach 2d live object so it will follow the
        /// value from this object.
        /// </summary>
        /// <param name="obj"> Info for liquid bar to follow. </param>
        public abstract void AttachInfo(JCS_LiquidBarInfo info);

        /// <summary>
        /// Current value in liquid bar.
        /// </summary>
        /// <returns> value in liquid bar </returns>
        public abstract float GetCurrentValue();

        /// <summary>
        /// Check if the value are able to cast.
        /// Mana value must higher than the cast value.
        /// </summary>
        /// <param name="val"> value to cast </param>
        /// <returns> true: able to cast, 
        ///           false: not able to cast </returns>
        public abstract bool IsAbleToCast(float val);

        /// <summary>
        /// Check if able to cast the spell, 
        /// if true cast it.
        /// </summary>
        /// <param name="val"> value to cast </param>
        public abstract bool IsAbleToCastCast(float val);

        /// <summary>
        /// Delta to current value in absolute negative value.
        /// </summary>
        public abstract void DeltaSub(float val);

        /// <summary>
        /// Delta to current value in absolute positive value.
        /// </summary>
        public abstract void DeltaAdd(float val);

        /// <summary>
        /// Delta the current value.
        /// </summary>
        /// <param name="deltaVal"> delta to current value's value </param>
        public abstract void DeltaCurrentValue(float deltaVal);

        /// <summary>
        /// zero the liquid bar.
        /// </summary>
        public abstract void Lack();

        /// <summary>
        /// Full the liquid bar.
        /// </summary>
        public abstract void Full();

        /// <summary>
        /// Set the value directly.
        /// </summary>
        /// <param name="val"> value to set </param>
        public abstract void SetCurrentValue(float val);

        /// <summary>
        /// Set the mininum value of this container.
        /// </summary>
        /// <param name="val"> value to min </param>
        public abstract void SetMinValue(float val);

        /// <summary>
        /// Set the maxinum value of this container.
        /// </summary>
        /// <param name="val"> value to max </param>
        public abstract void SetMaxValue(float val);

        /// <summary>
        /// Set the source sprite from the Image component.
        /// in Unity Engine.
        /// </summary>
        /// <param name="sprite"></param>
        public void SetInfoSprite(Sprite sprite)
        {
            if (mInfoImage == null)
            {
                JCS_Debug.LogError(
                    "Cannot set the sprite without the image component.");

                return;
            }

            mInfoImage.sprite = sprite;
        }
    }
}
