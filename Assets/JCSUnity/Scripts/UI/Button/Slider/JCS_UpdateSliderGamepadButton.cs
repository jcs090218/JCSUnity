/**
 * $File: JCS_UpdateSliderGamepadButton.cs $
 * $Date: 2022-09-03 22:51:49 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2022 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Update the slider value. (Gamepad)
    /// </summary>
    public class JCS_UpdateSliderGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_UpdateSliderGamepadButton)")]

        [Tooltip("Target value to assign to slider list.")]
        [SerializeField]
        private float mTargetValue = 1f;

        [Tooltip("List of effected sliders.")]
        public List<Slider> sliders = null;

        /* Setter & Getter */

        public float TargetValue { get { return this.mTargetValue; } set { this.mTargetValue = value; } }

        /* Functions */

        public override void OnClick()
        {
            UpdateSlidesrValue();
        }

        /// <summary>
        /// Update all sliders' value once.
        /// </summary>
        public void UpdateSlidesrValue()
        {
            foreach (var slider in sliders)
            {
                if (slider == null)
                    continue;

                slider.value = mTargetValue;
            }
        }
    }
}
