/**
 * $File: JCS_DialogueScript.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Interface of the dialogue script.
    /// </summary>
    public abstract class JCS_DialogueScript
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        [Tooltip("use to design the pages.")]
        public int Status = -1;

        //----------------------
        // Private Variables

        [Tooltip("")]
        [SerializeField]
        private Sprite mCenterSprite = null;

        [Tooltip("")]
        [SerializeField]
        private Sprite mLeftSprite = null;

        [Tooltip("")]
        [SerializeField]
        private Sprite mRightSprite = null;

        //----------------------
        // Protected Variables
        protected JCS_DialogueSystem mDialogueSystem = null;

        //========================================
        //      setter / getter
        //------------------------------
        protected JCS_DialogueSystem DialogueSystem { get { return this.mDialogueSystem; } }
        protected JCS_DialogueSystem jcsDs { get { return this.mDialogueSystem; } }

        public Sprite CenterSprite { get { return this.mCenterSprite; } }
        public Sprite LeftSprite { get { return this.mLeftSprite; } }
        public Sprite RightSprite { get { return this.mRightSprite; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Start()
        {
            // get dialogue system. (singleton)
            mDialogueSystem = JCS_UtilitiesManager.instance.GetDialogueSystem();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Starting point of the dialogue.
        /// </summary>
        /// <param name="mode"> 
        /// 0: quest completed
        /// 1: quest!
        /// </param>
        /// <param name="type">
        /// 
        /// </param>
        /// <param name="selection">
        /// 0: selection 0 / No button / Decline button
        /// 1: selection 1 / Yes button / Accept button
        /// 2: selection 2
        /// , etc...
        /// </param>
        public abstract void Action(int mode, int type, int selection);

        /// <summary>
        /// When the quest is ended already...
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="type"></param>
        /// <param name="selection"></param>
        public virtual void End(int mode, int type, int selection)
        {

        }

        /// <summary>
        /// Rset status.
        /// </summary>
        public void ResetAction()
        {
            Status = -1;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
