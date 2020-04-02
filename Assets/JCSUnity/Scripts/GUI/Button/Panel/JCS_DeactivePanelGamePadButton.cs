/**
 * $File: JCS_DeactivePanelGamePadButton.cs $
 * $Date: 2017-10-27 12:33:45 $
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
    /// Deactive panel button. (Game Pad)
    /// </summary>
    public class JCS_DeactivePanelGamePadButton
        : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_DeactivePanelGamePadButton) **")]

        [Tooltip("Play dialogue sound?")]
        [SerializeField]
        private bool mHideWithSound = true;

        [Tooltip("Panels to be deactive.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Tween Panels to be deactive.")]
        [SerializeField]
        private JCS_TweenPanel[] mTweenPanels = null;


        /* Setter & Getter */

        public bool HideithSound { get { return this.mHideWithSound; } set { this.mHideWithSound = value; } }

        /* Functions */

        public override void JCS_OnClickCallback()
        {
            foreach (JCS_DialogueObject panel in mDialogueObjects)
            {
                if (panel != null)
                {
                    if (mHideWithSound)
                        panel.HideDialogue();
                    else
                        panel.HideDialogueWithoutSound();
                }
            }

            foreach (JCS_TweenPanel panel in mTweenPanels)
            {
                if (panel != null)
                    panel.Deactive();
            }
        }
    }
}
