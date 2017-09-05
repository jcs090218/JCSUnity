/**
 * $File: JCS_DeactivePanelButton.cs $
 * $Date: 2017-09-04 16:37:26 $
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
    /// Deactive panel button.
    /// </summary>
    public class JCS_DeactivePanelButton
        : JCS_Button
    {
        [Header("** Runtime Variables (JCS_DeactivePanelButton) **")]

        [Tooltip("Panel to be active.")]
        [SerializeField]
        private JCS_TweenPanel mTweenPanel = null;


        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();

            if (mTweenPanel != null)
                mTweenPanel.Deactive();
        }
    }
}
