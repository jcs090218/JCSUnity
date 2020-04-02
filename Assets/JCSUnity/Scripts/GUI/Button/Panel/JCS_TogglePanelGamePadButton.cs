/**
 * $File: JCS_TogglePanelGamePadButton.cs $
 * $Date: 2020-04-02 12:37:09 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button that toggle one panel. (Game Pad)
    /// </summary>
    public class JCS_TogglePanelGamePadButton
        : JCS_ToggleGamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_TogglePanelGamePadButton) **")]

        [Tooltip("Panel you want to toggle.")]
        [SerializeField]
        private JCS_DialogueObject mDialogueObject = null;

        [Tooltip("Panel you want to toggle.")]
        [SerializeField]
        private JCS_TweenPanel mTweenPanel = null;

        /* Setter & Getter */

        public JCS_DialogueObject DialogueObject { get { return this.mDialogueObject; } }
        public JCS_TweenPanel TweenPanel { get { return this.mTweenPanel; } }

        /* Functions */

        protected virtual void Start()
        {
            acitveFunc += ActivePanel;
            deactiveFunc += DeactivePanel;
        }

        private void ActivePanel()
        {
            if (!CheckPanel())
                return;

            if (mDialogueObject != null)
                mDialogueObject.ShowDialogue();

            if (mTweenPanel != null)
                mTweenPanel.Active();
        }

        private void DeactivePanel()
        {
            if (!CheckPanel())
                return;

            if (mDialogueObject != null)
                mDialogueObject.HideDialogue();

            if (mTweenPanel != null)
                mTweenPanel.Deactive();
        }

        /// <summary>
        /// Check if panel valid to toggle.
        /// </summary>
        /// <returns></returns>
        private bool CheckPanel()
        {
            if (mDialogueObject == null && mTweenPanel == null)
            {
                JCS_Debug.LogWarning("Toggle panel must assign either one `dialogue object` or `tween panel`");
                return false;
            }
            return true;
        }
    }
}
