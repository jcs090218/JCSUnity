/**
 * $File: JCS_2DAnimation.cs $
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
    /// Handle frame by frame animation in the simple way.
    /// </summary>
    public class JCS_2DAnimation
        : JCS_UnityObject
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // animator using this animation?
        private JCS_2DAnimator mJCS2DAnimator = null;


#if (UNITY_EDITOR)
        [Header("** Helper Variables Variables (JCS_I2DAnimator) **")]

        [SerializeField]
        private bool mTestWithKey = true;
#endif


        [Header("** Check Variables (JCS_Animation) **")]

        [Tooltip("Frame this animation current playing.")]
        [SerializeField]
        private int mCurrentPlayingFrame = 0;

        [Tooltip("Maxinum frame in the animation.")]
        [SerializeField]
        private int mMaxFrame = 0;

        // flag to know if the animation is done.
        // ATTENTION(jenchieh): this cannot be use with loop.
        [SerializeField]
        private bool mDonePlaying = false;


        [Header("** Initialize Variables (JCS_Animation) **")]

        [Tooltip("When does this animation start the frame?")]
        [SerializeField]
        private int mStartingFrame = 0;


        [Header("** Runtime Variables (JCS_Animation) **")]

        [Tooltip("Do play the animation?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Play the animation on awake time?")]
        [SerializeField]
        private bool mPlayOnAwake = true;

        [Tooltip("Did the animation loop?")]
        [SerializeField]
        private bool mLoop = true;

        [Tooltip("")]
        [SerializeField]
        private float mFramePerSec = 0.1f;

        // timer to decide next frame.
        private float mFrameTimer = 0;

        [Tooltip("Drag all the frame here, in order.")]
        [SerializeField]
        private Sprite[] mAnimFrames = null;

        [Tooltip(@"This will times the animation time by 
this, default is 1.")]
        [SerializeField] [Range(0, 5)]
        private float mAnimationTimeProduction = 1;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool PlayOnAwake { get { return this.mPlayOnAwake; } set { this.mPlayOnAwake = value; } }
        public int CurrentPlayingFrame { get { return this.mCurrentPlayingFrame; }
            set
            {
                this.mCurrentPlayingFrame = value;

                // make sure animation in the valid frame.
                PutAnimInFrame();
            }
        }
        public bool Loop { get { return this.mLoop; } set { this.mLoop = value; } }
        // Is the animation done playing?
        public bool DonePlaying { get { return this.mDonePlaying; } }
        public void SetJCS2DAnimator(JCS_2DAnimator jcs2dAnimator) { this.mJCS2DAnimator = jcs2dAnimator; }
        public float AnimationTimeProduction { get { return this.mAnimationTimeProduction; } }

        // Bind.
        public void SetAnimationFrame(Sprite[] frames)
        {
            this.mAnimFrames = frames;
            Awake();
        }
        public Sprite[] GetAnimationFrame()
        {
            return this.mAnimFrames;
        }
        public float FramePerSec { get { return this.mFramePerSec; } set { this.mFramePerSec = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            /**
             * @note: Recommanded tree setting will be like.
             * 
             * JCS_2DAnimator (SpriteRenderer component located here!)
             *   -> JCS_2DAnimation (Own animation sequence.)
             *   -> JCS_2DAnimation (Own animation sequence.)
             *   -> JCS_2DAnimation (Own animation sequence.)
             */
            // Even we get the the sprite renderer component, 
            // JCS_2DAnimator will override this component if this 
            // component(JCS_2DAnimation) are in the array of the 
            // JCS_2DAnimator component.
            UpdateUnityData();

            mCurrentPlayingFrame = mStartingFrame;

            // update max frame count.
            UpdateMaxFrame();
        }

        private void Start()
        {
            // check if play on awake time?
            if (mPlayOnAwake)
                Play();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            RunAnimation();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKey(KeyCode.Q))
                Play();

            if (Input.GetKey(KeyCode.W))
                Stop();

            if (Input.GetKey(KeyCode.E))
                Pause();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Update the maxinum frame count from the 
        /// anim frame sequence.
        /// </summary>
        public void UpdateMaxFrame()
        {
            if (mAnimFrames == null)
                return;

            this.mMaxFrame = this.mAnimFrames.Length;
        }

        /// <summary>
        /// Play the animation in current frame.
        /// </summary>
        /// <param name="statFrame"></param>
        public void Play(int startFrame = -1, bool ignoreActive = false)
        {
            if (!ignoreActive)
            {
                // if already playing, do nothing.
                if (mActive)
                    return;
            }

            // starting of playing the animation, 
            // animation cannot be done playing!
            mDonePlaying = false;

            // set the current animation.
            if (startFrame != -1)
            {
                mCurrentPlayingFrame = startFrame;

                // reset timer.
                mFrameTimer = 0;
            }

            // make sure animation in the range of valid
            // animation frame count.
            PutAnimInFrame();

            // play the frame immediatly
            PlayFrame();

            // start the animation.
            mActive = true;
        }

        /// <summary>
        /// Pause the animation by frame?
        /// </summary>
        public void Pause(int frame = -1)
        {
            // stop at that frame.
            if (frame != -1)
            {
                // set the current animation.
                mCurrentPlayingFrame = frame;

                // make sure animation in the range of valid
                // animation frame count.
                PutAnimInFrame();

                // play this frame.
                PlayFrame();
            }

            // stop the animation.
            mActive = false;
        }

        /// <summary>
        /// Stop the animation, and set the sprite to null.
        /// </summary>
        public void Stop()
        {
            // set the sprite to null.
            mSpriteRenderer.sprite = null;

            // stop the animation.
            mActive = false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Put the animation in the frame count.
        /// </summary>
        private void PutAnimInFrame()
        {
            if (mCurrentPlayingFrame < 0)
                this.mCurrentPlayingFrame = 0;
            else if (mCurrentPlayingFrame >= mMaxFrame - 1)
                this.mCurrentPlayingFrame = mMaxFrame - 1;
        }


        /// <summary>
        /// Default, use current playing frame index.
        /// </summary>
        private void PlayFrame()
        {
            PlayFrame(mCurrentPlayingFrame);
        }

        /// <summary>
        /// set the frame by index.
        /// </summary>
        private void PlayFrame(int frame)
        {
            PutAnimInFrame();

            // set the current sprite.
            LocalSprite = mAnimFrames[mCurrentPlayingFrame];
        }

        /// <summary>
        /// Main algorithm to display animation frame 
        /// by frame.
        /// </summary>
        private void RunAnimation()
        {
            // check if the animation active.
            if (!mActive)
                return;

            // start the timer.
            mFrameTimer += Time.deltaTime;

            // get the time per seconds.
            // NOTE(jenchieh): multiple you own animate production first.
            float timePerSec = mFramePerSec * mAnimationTimeProduction;

            // if there is animator taking over this animation.
            // times the production time!
            if (mJCS2DAnimator != null)
                timePerSec = mFramePerSec * mJCS2DAnimator.AnimationTimeProduction;

            // check timer reach the next frame time.
            if (mFrameTimer < timePerSec)
                return;

            // add one frame, ready for next frame.
            ++mCurrentPlayingFrame;

            // reset the timer if reach the max frame.
            if (mCurrentPlayingFrame == mMaxFrame)
            {
                if (mLoop)
                    mCurrentPlayingFrame = 0;
                else
                {
                    // current frame will just be the last frame.
                    mCurrentPlayingFrame = mMaxFrame;
                }

                // set the flag up.
                mDonePlaying = true;
            }

            // set the current frame.
            PlayFrame(mCurrentPlayingFrame);

            // reset timer.
            mFrameTimer = 0;
        }

    }
}
