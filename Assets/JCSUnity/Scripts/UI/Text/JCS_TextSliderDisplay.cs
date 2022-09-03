/**
 * $File: JCS_TextSliderDisplay.cs $
 * $Date: 2022-09-04 01:10:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Text that display slider's value.
    /// </summary>
    public class JCS_TextSliderDisplay : JCS_TextObject
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_TextSliderDisplay) **")]

        [Tooltip("To update the text along with this slider's value.")]
        [SerializeField]
        private Slider mSlider = null;

        /* Setter & Getter */

        public Slider slider { get { return this.mSlider; } set { this.mSlider = value; } }

        /* Functions */

        private void Update()
        {
            if (mSlider == null)
                return;

            this.text = mSlider.value.ToString();
        }
    }
}
