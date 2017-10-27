/**
 * $File: JCS_ActivePanelGamePadButton.cs $
 * $Date: 2017-10-27 12:32:31 $
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
    /// Active panel button. (Game Pad)
    /// </summary>
    public class JCS_ActivePanelGamePadButton
        : JCS_GamePadButton
    {
        [Header("** Runtime Variables (JCS_ActivePanelGamePadButton) **")]

        [Tooltip("Play dialogue sound?")]
        [SerializeField]
        private bool mShowWithSound = true;

        [Tooltip("Panels to be active.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Tween Panels to be active.")]
        [SerializeField]
        private JCS_TweenPanel[] mTweenPanels = null;


        public bool ShowWithSound { get { return this.mShowWithSound; } set { this.mShowWithSound = value; } }


        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();

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
                    panel.Active();
            }
        }
    }
}
