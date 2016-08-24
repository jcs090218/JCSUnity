/**
 * $File: JCS_2DLiveObjectAnimator.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_2DLiveObjectAnimator
        : JCS_2DAnimator
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        [Header("** Check Variables (JCS_2DLiveObjectAnimator) **")]
        [SerializeField]
        protected string mCurrentStateName = "";
        [SerializeField]
        protected JCS_LiveObjectState mCurrentState
            = JCS_LiveObjectState.STAND;

        [Header("** Initialize Variables (JCS_2DLiveObjectAnimator) **")]
        // Please set the int inside the "Animator" window in Unity, 
        // Control the animation state with code are much easier to programmer.
        [SerializeField] protected string mAnimationState = "State";

        [SerializeField] protected float mAnimationTimer = 0.0f;
        protected AnimatorStateInfo mAnimatorStateInfo;
        [SerializeField] protected float mAnimationOffset = 0.05f;

        [Tooltip("Plz use %jcs as the state variable name.")]
        [SerializeField]
        protected string mFullClipStateName = "Player_01_%jcs";
        protected bool mEndAttackStage = true;

        [Tooltip(@"Override the current animation, start 
from the beginning.")] [SerializeField]
        protected bool mOverrideAnim = false;

        //========================================
        //      setter / getter
        //------------------------------
        public AnimatorStateInfo GetAnimatorStateInfo() { return this.mAnimatorStateInfo; }
        
        public string GetAnimationState() { return this.mAnimationState; }
        public JCS_LiveObjectState GetCurrentAnimationState() { return this.mCurrentState; }
        public bool GetEndAttackStage() { return this.mEndAttackStage; }

        //========================================
        //      Unity's function
        //------------------------------

        private void Update()
        {
            if (mEndAttackStage)
                return;

            if (IsInState(JCS_LiveObjectState.RAND_ATTACK))
            {
                mAnimationTimer += Time.deltaTime;

                if (mAnimationTimer >= mAnimatorStateInfo.length + mAnimationOffset)
                {
                    mAnimationTimer = 0;
                    mEndAttackStage = true;
                }
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Animation design here...
        /// </summary>
        /// <param name="state"></param>
        public override void DoAnimation(JCS_LiveObjectState state = JCS_LiveObjectState.STAND)
        {
            // return if the animation are already playing
            if (mCurrentState == state)
            {
                if (mOverrideAnim)
                    PlayAtBeginning();

                return;
            }

            mCurrentStateName = GetStateName(state);

            GetAnimator().SetInteger(GetAnimationState(), (int)state);

            this.mCurrentState = state;

            if (mCurrentState == JCS_LiveObjectState.RAND_ATTACK)
                mEndAttackStage = false;
        }

        /// <summary>
        /// Check this player in that state!
        /// </summary>
        /// <param name="state"> state to check </param>
        /// <returns> true: in the state, false: not in the state </returns>
        public bool IsInState(JCS_LiveObjectState state)
        {
            // first check the self-define one.
            if (mCurrentState != state)
                return false;

            // then check Unity's Animator state.
            mAnimatorStateInfo = GetAnimator().GetCurrentAnimatorStateInfo(0);

            return mAnimatorStateInfo.IsName(mCurrentStateName);
        }

        /// <summary>
        /// Play the animation start of the 
        /// current aniamtion.
        /// </summary>
        public void PlayAtBeginning()
        {
            mAnimator.Play(mCurrentStateName, -1, 0);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string GetStateName(JCS_LiveObjectState state)
        {
            string stateName = mFullClipStateName;
            string swapName = "";

            switch (state)
            {
                case JCS_LiveObjectState.STAND:
                    swapName = "stand";
                    break;
                case JCS_LiveObjectState.WALK:
                    swapName = "walk";
                    break;
                case JCS_LiveObjectState.RAND_ATTACK:
                    swapName = "attack";
                    break;
                case JCS_LiveObjectState.JUMP:
                    swapName = "jump";
                    break;
                case JCS_LiveObjectState.PRONE:
                    swapName = "prone";
                    break;
                case JCS_LiveObjectState.ALERT:
                    swapName = "alert";
                    break;
                case JCS_LiveObjectState.FLY:
                    swapName = "fly";
                    break;
                case JCS_LiveObjectState.LADDER:
                    swapName = "ladder";
                    break;
                case JCS_LiveObjectState.ROPE:
                    swapName = "rope";
                    break;
                case JCS_LiveObjectState.SIT:
                    swapName = "sit";
                    break;
                case JCS_LiveObjectState.HIT:
                    swapName = "hit";
                    break;
                case JCS_LiveObjectState.DANCE:
                    swapName = "dance";
                    break;
                case JCS_LiveObjectState.SWIM:
                    swapName = "swim";
                    break;
                case JCS_LiveObjectState.DIE:
                    swapName = "die";
                    break;
                case JCS_LiveObjectState.GHOST:
                    swapName = "ghost";
                    break;
            }

            string[] words;

            words = stateName.Split(new[] { "%jcs" }, StringSplitOptions.None);

            stateName = words[0] + swapName + words[1];

            return stateName;
        }

    }
}
