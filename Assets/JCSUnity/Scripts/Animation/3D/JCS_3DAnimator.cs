/**
 * $File: JCS_3DAnimator.cs $
 * $Date: 2016-09-28 13:42:28 $
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
    /// 
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_3DAnimator
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables
        protected Animator mAnimator = null;

        //========================================
        //      setter / getter
        //------------------------------
        public Animator GetAnimator() { return this.mAnimator; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// play the animation in current frame.
        /// </summary>
        public virtual void PlayAnimationInFrame()
        {
            GetAnimator().enabled = true;
        }

        /// <summary>
        /// Stop animation in current frame.
        /// </summary>
        public virtual void StopAnimationInFrame()
        {
            GetAnimator().enabled = false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
