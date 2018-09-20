/**
 * $File: JCS_DestroyAnimEndEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// Play Animator and destroy after it 
    /// played animation depends on loop times.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_DestroyAnimEndEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private Animator mAnimator = null;
        private float mAnimationTimer = 0.0f;
        [SerializeField] private int mLoopTimes = 1;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int LoopTimes { get { return this.mLoopTimes; } set { this.mLoopTimes = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

            mAnimationTimer += Time.deltaTime;

            if (mAnimationTimer > animatorStateInfo.length * mLoopTimes)
            {
                Destroy(this.gameObject);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
