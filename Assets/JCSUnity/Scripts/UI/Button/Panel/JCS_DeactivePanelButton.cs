/**
 * $File: JCS_DeactivePanelButton.cs $
 * $Date: 2017-09-04 16:37:26 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Deactive panel button.
    /// </summary>
    public class JCS_DeactivePanelButton : JCS_Button
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_DeactivePanelButton)")]

        [Tooltip("Panels to be deactive.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Tween Panels to be deactive.")]
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

        public override void OnClick()
        {
            JCS_UIUtil.DeactivePanels(mDialogueObjects, mPlaySound);
            JCS_UIUtil.DeactivePanels(mTweenPanels);
        }
    }
}
