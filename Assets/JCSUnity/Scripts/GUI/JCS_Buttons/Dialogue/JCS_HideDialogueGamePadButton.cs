/**
 * $File: JCS_HideDialogueGamePadButton.cs $
 * $Date: 2017-10-27 12:25:34 $
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
    /// Click the button to hide multiple dialogue. (Game Pad)
    /// </summary>
    public class JCS_HideDialogueGamePadButton
        : JCS_GamePadButton
    {
        [Header("** Runtime Variables (JCS_HideDialogueGamePadButton) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject[] mDialoguesToHide = null;

        [Tooltip("")]
        [SerializeField]
        private bool mHideWithSound = true;


        public JCS_DialogueObject[] DialoguesToHide { get { return this.mDialoguesToHide; } }
        public bool HideithSound { get { return this.mHideWithSound; } set { this.mHideWithSound = value; } }


        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();


            foreach (JCS_DialogueObject dialogue in mDialoguesToHide)
            {
                if (mHideWithSound)
                    dialogue.HideDialogue();
                else
                    dialogue.HideDialogueWithoutSound();
            }
        }
    }
}
