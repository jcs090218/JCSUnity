/**
 * $File: JCS_2DAnimator.cs $
 * $Date: 2017-03-13 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{

    /// <summary>
    /// Hold sequence of animation and play it by condition.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_2DAnimator 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private SpriteRenderer mSpriteRenderer = null;


        [Header("** Check Variables Variables (JCS_I2DAnimator) **")]

        [Tooltip("Current animation id.")]
        [SerializeField]
        private int mCurrentAnimId = 0;

        [Tooltip("Current playing animation.")]
        [SerializeField]
        private JCS_2DAnimation mCurrentAnimation = null;

        [Tooltip("Maxinum frame in the animation.")]
        [SerializeField]
        private int mMaxAnimCount = 0;

        // Current animation stack.
        private JCS_2DAnimation mStackAnimation = null;
        private int mStackAnimId = -1;


        [Header("** Runtime Variables Variables (JCS_I2DAnimator) **")]

        [Tooltip(@"This will times the animation time by this, 
default is 1.")]
        [SerializeField]
        [Range(0, 5)]
        private float mAnimationTimeProduction = 1;

        [Tooltip("All the animation possible to this object.")]
        [SerializeField]
        private JCS_2DAnimation[] mAnimations = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float AnimationTimeProduction { get { return this.mAnimationTimeProduction; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();

            // update the max animation count.
            UpdateMaxAnimCount();

            // active this animation.
            ActiveOneAnimation(0);
        }

        private void Start()
        {
            // override the component in the animation array.
            foreach (JCS_2DAnimation anim in mAnimations)
            {
                // let the animation know there are animator controlling 
                // the animation.
                anim.SetJCS2DAnimator(this);

                // let all animation know there are sprite renderer 
                // take over what they had previous 'SpriteRenderer' 
                // component.
                anim.SetSpriteRenderer(this.mSpriteRenderer);

                anim.PlayOnAwake = false;
            }
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            DoPlayOneShot();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (Input.GetKey(KeyCode.A))
                SwitchAnimation(0, true);

            if (Input.GetKey(KeyCode.S))
                SwitchAnimation(1, false);

            if (Input.GetKey(KeyCode.D))
                SwitchAnimation(2, true);

            if (Input.GetKey(KeyCode.G))
                PlayOneShot(0);
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Play the animation base on the animation ID.
        /// </summary>
        /// <param name="id"> animation index in array. </param>
        /// <param name="over"> override the animation? </param>
        public void SwitchAnimation(int id, bool over = false)
        {
            if (!over)
            {
                // if same id, return.
                if (mCurrentAnimId == id)
                    return;
            }

            mCurrentAnimId = id;

            PutAnimaIdInSequence();

            // get the current playing animation.
            mCurrentAnimation = mAnimations[mCurrentAnimId];

            if (mCurrentAnimation == null)
            {
                JCS_Debug.JcsErrors(
                    this, "Swtich animation failed cuz of null reference animation assigned...");
                return;
            }

            // active this animation.
            ActiveOneAnimation(id);

            // restart the animation.
            mCurrentAnimation.Play();
        }

        /// <summary>
        /// Play one animation once and go back to 
        /// original animation.
        /// </summary>
        /// <param name="id"> animation index in array. </param>
        /// <param name="over"> override the play one shot action? </param>
        public void PlayOneShot(int id, bool over = false)
        {
            // 如果要蓋過, 就不檢查了.
            if (!over)
            {
                // check if play one shot still do the stuff.
                if (mStackAnimation != null)
                    return;
            }

            // push the animation to stack.
            mStackAnimation = mCurrentAnimation;
            mStackAnimId = mCurrentAnimId;

            // play the newer animation.
            SwitchAnimation(id);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Update the maxinum frame count from the 
        /// anim frame sequence.
        /// </summary>
        public void UpdateMaxAnimCount()
        {
            this.mMaxAnimCount = this.mAnimations.Length;
        }

        /// <summary>
        /// Make sure the animation we are playing 
        /// are not null reference.
        /// </summary>
        private void PutAnimaIdInSequence()
        {
            if (mCurrentAnimId < 0)
                this.mCurrentAnimId = 0;
            else if (mCurrentAnimId >= mMaxAnimCount)
                this.mCurrentAnimId = mMaxAnimCount;
        }

        /// <summary>
        /// Active on animation by ID.
        /// </summary>
        /// <param name="id"> animation index in array. </param>
        private void ActiveOneAnimation(int id)
        {
            foreach (JCS_2DAnimation anim in mAnimations)
            {
                // disable all
                anim.Active = false;
            }

            // only active playing target animation.
            mAnimations[id].Active = true;
        }

        /// <summary>
        /// Check if play one shot valid.
        /// If valid, do the play one shot algorithm.
        /// </summary>
        private void DoPlayOneShot()
        {
            /**
             * Check stack empty, if not empty meaning
             * there are PlayOneShot() animation going on.
             * 
             * ATTENTION(jenchieh): make sure to clean up the 
             * stack after the animatin is played.
             */
            if (mStackAnimation == null)
                return;


            // clean up the stack after the animation is 
            // done playing.

            if (mCurrentAnimation.DonePlaying)
            {
                // switch the animation back to original 
                // animation by stack id.
                SwitchAnimation(mStackAnimId);

                // clean up the stack pointer.
                mStackAnimation = null;
                // clean up stack animation id.
                mStackAnimId = -1;
            }
        }
    }
}
