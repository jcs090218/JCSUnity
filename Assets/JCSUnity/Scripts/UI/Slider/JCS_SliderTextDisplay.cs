/**
 * $File: JCS_SliderTextDisplay.cs $
 * $Date: 2022-09-04 01:10:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// A list of slider text display type.
    /// </summary>
    public enum SliderTextDisplay
    {
        CUSTOM,
        VALUE,
        MIN_VALUE,
        MAX_VALUE,
    }

    /// <summary>
    /// Text that display slider's value.
    /// </summary>
    public class JCS_SliderTextDisplay : JCS_TextObject
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_TextSliderDisplay)")]

        [Tooltip("To update the text along with this slider's value.")]
        [SerializeField]
        private Slider mSlider = null;

        [Tooltip("Text display type.")]
        [SerializeField]
        private SliderTextDisplay mTextDisplay = SliderTextDisplay.VALUE;

        [Tooltip("Place you want to round the decimal.")]
        [SerializeField]
        [Range(0, 15)]
        private int mRoundPlace = 2;

        [Tooltip("Display with custom text.")]
        [SerializeField]
        private string mCustomFormat = "{0} / {1}";

        /* Setter & Getter */

        public Slider slider { get { return this.mSlider; } set { this.mSlider = value; } }
        public SliderTextDisplay TextDisplay { get { return this.mTextDisplay; } set { this.mTextDisplay = value; } }
        public int RoundPlace { get { return this.mRoundPlace; } set { this.mRoundPlace = value; } }
        public string CustomFormat { get { return this.mCustomFormat; } set { this.mCustomFormat = value; } }

        /* Functions */

        private void Awake()
        {
            AddListener();
        }

        private void OnValidate()
        {
            AddListener();
        }

        private void AddListener()
        {
            if (mSlider == null)
                return;

            mSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });

            // Call once.
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            if (mTextDisplay == SliderTextDisplay.CUSTOM)
            {
                this.text = string.Format(mCustomFormat,
                    GetRoundValue(SliderTextDisplay.VALUE),
                    GetRoundValue(SliderTextDisplay.MAX_VALUE),
                    GetRoundValue(SliderTextDisplay.MIN_VALUE));

                return;
            }

            double val = GetRoundValue(mTextDisplay);

            this.text = val.ToString();
        }

        /// <summary>
        /// Like the function `GetValue` but round the result.
        /// </summary>
        private double GetRoundValue(SliderTextDisplay displayType)
        {
            return Math.Round(GetValue(displayType), mRoundPlace);
        }

        /// <summary>
        /// Return the slider text display type.
        /// </summary>
        private double GetValue(SliderTextDisplay displayType)
        {
            switch (displayType)
            {
                case SliderTextDisplay.VALUE:
                    return mSlider.value;
                case SliderTextDisplay.MIN_VALUE:
                    return mSlider.minValue;
                case SliderTextDisplay.MAX_VALUE:
                    return mSlider.maxValue;
            }

            return 0.0f;
        }
    }
}
