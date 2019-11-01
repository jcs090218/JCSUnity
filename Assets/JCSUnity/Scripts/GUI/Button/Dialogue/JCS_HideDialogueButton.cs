/**
 * $File: JCS_HideDialogueButton.cs $
 * $Date: 2017-02-24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Click the button to hide multiple dialogue.
    /// </summary>
    public class JCS_HideDialogueButton
        : JCS_Button
    {
        [Header("** Runtime Variables (JCS_HideDialogueButton) **")]

        [Tooltip("List of dialogue panels you want to hide.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialoguesToHide = null;

        [Tooltip("When hiding play the sound.")]
        [SerializeField]
        private bool mHideWithSound = false;


        public JCS_DialogueObject[] DialoguesToHide { get { return this.mDialoguesToHide; } }
        public bool HideithSound { get { return this.mHideWithSound; } set { this.mHideWithSound = value; } }


        public override void JCS_OnClickCallback()
        {
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
