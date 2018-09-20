/**
 * $File: JCS_DestroyAnimBackForthEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;


namespace JCSUnity
{
    /// <summary>
    /// After playing animation back and forth once, 
    /// then destroy...
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class JCS_DestroyAnimBackForthEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private Animator mAnimator = null;
        private float mAnimationTimer = 0.0f;

        private bool mPlayBack = false;
        private bool mPlayForth = false;

        private AnimatorStateInfo mAnimtorStateInfo;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mAnimator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            mAnimtorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            if (JCS_Input.GetKeyDown(KeyCode.I))
                PlayForth();
            if (JCS_Input.GetKeyDown(KeyCode.O))
                PlayBack();
#endif


            // play forth first
            if (mPlayForth)
            {
                mAnimationTimer += Time.deltaTime;

                if (mAnimationTimer > mAnimtorStateInfo.length)
                {
                    mPlayForth = false;

                    // since we are using the same timer, 
                    // we simply set the timer to zero in order to 
                    // calculate the back animation time.
                    mAnimationTimer = 0;

                    // disable the animator
                    mAnimator.enabled = false;
                }
            }

            // play back second
            if (mPlayBack)
            {
                mAnimationTimer += Time.deltaTime;

                if (mAnimationTimer > mAnimtorStateInfo.length)
                {
                    mPlayBack = false;

                    mAnimationTimer = 0;

                    Destroy(this.gameObject);
                }
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Play the animation forth once.
        /// </summary>
        public void PlayForth()
        {
            mPlayForth = true;
            mPlayBack = false;

            mAnimator.enabled = true;

            mAnimationTimer = 0;

            // in Animator Window create a float variable call 
            // as the following name.
            mAnimator.SetFloat("AnimSpeed", 1);

            // get the name of this clip
            string tempClipName = mAnimator.runtimeAnimatorController.animationClips[0].ToString();
            int index = tempClipName.IndexOf(" ");
            string clipName = tempClipName.Substring(0, index);


            mAnimator.Play(clipName,   0);
        }

        /// <summary>
        /// Play the animation back once.
        /// </summary>
        public void PlayBack()
        {
            mPlayBack = true;
            mPlayForth = false;

            mAnimator.enabled = true;

            mAnimationTimer = 0;

            // in Animator Window create a float variable call 
            // as the following name.
            mAnimator.SetFloat("AnimSpeed", -1);

            // get the name of this clip
            string tempClipName = mAnimator.runtimeAnimatorController.animationClips[0].ToString();
            int index = tempClipName.IndexOf(" ");
            string clipName = tempClipName.Substring(0, index);


            mAnimator.Play(clipName, -1, mAnimtorStateInfo.normalizedTime);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
