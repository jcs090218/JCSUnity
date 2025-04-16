/**
 * $File: JCS_TogglePanelGamepadButton.cs $
 * $Date: 2020-04-02 12:37:09 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button that toggle one panel. (Gamepad)
    /// </summary>
    public class JCS_TogglePanelGamepadButton : JCS_ToggleGamepadButton
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_TogglePanelGamepadButton)")]

        [Tooltip("Panel (dialogue) you want to toggle.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Panel (tween) you want to toggle.")]
        [SerializeField]
        private JCS_TweenPanel[] mTweenPanels = null;

        [Tooltip("Play dialogue sound.")]
        [SerializeField]
        private bool mPlaySound = true;

        /* Setter & Getter */

        public JCS_DialogueObject[] DialogueObjects { get { return this.mDialogueObjects; } }
        public JCS_TweenPanel[] TweenPanels { get { return this.mTweenPanels; } }

        public bool PlaySound { get { return this.mPlaySound; } set { this.mPlaySound = value; } }

        /* Functions */

        protected virtual void Start()
        {
            onActive += ActivePanel;
            onDeactive += DeactivePanel;
        }

        private void ActivePanel()
        {
            JCS_UIUtil.ActivePanels(mDialogueObjects, mPlaySound);
            JCS_UIUtil.ActivePanels(mTweenPanels);
        }

        private void DeactivePanel()
        {
            JCS_UIUtil.DeactivePanels(mDialogueObjects, mPlaySound);
            JCS_UIUtil.DeactivePanels(mTweenPanels);
        }
    }
}
