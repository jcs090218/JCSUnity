/**
 * $File: JCS_ShowDialogueGamePadButton.cs $
 * $Date: 2017-10-27 12:27:23 $
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
    /// Click the button to show multiple dialogue. (Gamepad)
    /// </summary>
    public class JCS_ShowDialogueGamePadButton
        : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ShowDialogueGamePadButton) **")]

        [Tooltip("List of dialogue panels you want to show.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialoguesToShow = null;

        [Tooltip("When showing play the sound.")]
        [SerializeField]
        private bool mShowWithSound = false;


        /* Setter & Getter */

        public JCS_DialogueObject[] DialoguesToShow { get { return this.mDialoguesToShow; } }
        public bool ShowWithSound { get { return this.mShowWithSound; } set { this.mShowWithSound = value; } }


        /* Functions */

        public override void JCS_OnClickCallback()
        {
            foreach (JCS_DialogueObject dialogue in mDialoguesToShow)
            {
                if (mShowWithSound)
                    dialogue.ShowDialogue();
                else
                    dialogue.ShowDialogueWithoutSound();
            }
        }
    }
}
