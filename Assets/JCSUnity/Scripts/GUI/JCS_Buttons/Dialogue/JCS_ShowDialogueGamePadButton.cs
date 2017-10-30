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
    /// Click the button to show multiple dialogyue. (Game Pad)
    /// </summary>
    public class JCS_ShowDialogueGamePadButton
        : JCS_GamePadButton
    {

        [Header("** Runtime Variables (JCS_ShowDialogueGamePadButton) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject[] mDialoguesToShow = null;

        [Tooltip("Play dialogue sound?")]
        [SerializeField]
        private bool mShowWithSound = false;


        public JCS_DialogueObject[] DialoguesToShow { get { return this.mDialoguesToShow; } }
        public bool ShowWithSound { get { return this.mShowWithSound; } set { this.mShowWithSound = value; } }


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
