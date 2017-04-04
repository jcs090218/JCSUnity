/**
 * $File: JCS_ShowDialogueButton.cs $
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
    /// Click the button to show multiple dialogyue.
    /// </summary>
    public class JCS_ShowDialogueButton
        : JCS_Button
    {

        [Header("** Runtime Variables (JCS_ShowDialogueButton) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_DialogueObject[] mDialoguesToShow = null;

        [Tooltip("Play dialogue sound?")]
        [SerializeField]
        private bool mShowWithSound = false;


        public JCS_DialogueObject[] DialoguesToShow { get { return this.mDialoguesToShow; } }

        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();


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
