/**
 * $File: JCS_ActivePanelButton.cs $
 * $Date: 2017-09-04 16:32:23 $
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
    /// Active panel button.
    /// </summary>
    public class JCS_ActivePanelButton
        : JCS_Button
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ActivePanelButton) **")]

        [Tooltip("Play dialogue sound?")]
        [SerializeField]
        private bool mShowWithSound = true;

        [Tooltip("Panels to be active.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Tween Panels to be active.")]
        [SerializeField]
        private JCS_TweenPanel[] mTweenPanels = null;


        /* Setter & Getter */

        /* Functions */

        public override void JCS_OnClickCallback()
        {
            foreach (JCS_DialogueObject panel in mDialogueObjects)
            {
                if (panel != null)
                {
                    if (mShowWithSound)
                        panel.ShowDialogue();
                    else
                        panel.ShowDialogueWithoutSound();
                }
            }

            foreach (JCS_TweenPanel panel in mTweenPanels)
            {
                if (panel != null)
                {
                    panel.Active();
                }
            }
        }
    }
}
