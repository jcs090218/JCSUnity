/**
 * $File: JCS_CharacterAnimator.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    // Player must have the animation implemented
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(JCS_OrderLayerObject))]
    public class JCS_CharacterAnimator 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField]
        private JCS_PlayerState mCurrentState = JCS_PlayerState.STAND;

        //----------------------
        // Protected Variables
        protected Animator mAnimator = null;


        // Please set the int inside the "Animator" window in Unity, 
        // Control the animation state with code are much easier to programmer.
        [SerializeField] protected string mAnimationState = "State";

        [SerializeField] private float mAnimationTimer = 0.0f;
        private AnimatorStateInfo mAnimatorStateInfo;
        [SerializeField] private float mAnimationOffset = 0.05f;
        [SerializeField] private string mFullClipState = "Player_01_Attack";
        private bool mEndAttackStage = true;

        //========================================
        //      setter / getter
        //------------------------------
        public Animator GetAnimator() { return this.mAnimator; }
        public string GetAnimationState() { return this.mAnimationState; }
        public JCS_PlayerState GetCurrentAnimationState() { return this.mCurrentState; }
        public bool GetEndAttackStage() { return this.mEndAttackStage; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mAnimator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            if (mEndAttackStage)
                return;

            mAnimatorStateInfo = GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (mAnimatorStateInfo.IsName(mFullClipState))
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
        public void DoAnimation(JCS_PlayerState state = JCS_PlayerState.STAND)
        {
            // return if the animation are already playing
            if (mCurrentState == state)
                return;

            GetAnimator().SetInteger(GetAnimationState(), (int)state);

            this.mCurrentState = state;

            if (mCurrentState == JCS_PlayerState.ATTACK)
                mEndAttackStage = false;
        }

        public void StopAnimationInFrame()
        {
            GetAnimator().enabled = false;
        }

        public void PlayAnimationInFrame()
        {
            GetAnimator().enabled = true;
        }


        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private AnimationClip FindClip(AnimationClip[] arr, string animClipName)
        {

            for (int index = 0;
                index < arr.Length;
                ++index)
            {
                if (arr[index].name == animClipName)
                    return arr[index];
            }


            JCS_GameErrors.JcsErrors("JCS_CharacterAnimator", -1, "No animation clip found...");
            return null;
        }

    }
}
