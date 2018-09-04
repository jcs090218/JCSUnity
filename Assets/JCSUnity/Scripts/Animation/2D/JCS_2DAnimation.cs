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
    public delegate void DonePlayingAnimation();
    public delegate void PlayFrameCallback();

    /// <summary>
    /// Handle frame by frame animation in the simple way.
    /// </summary>
    public class JCS_2DAnimation
        : JCS_UnityObject
    {
        //----------------------
        // Public Variables

        // call back when done playing the animation.
        public DonePlayingAnimation donePlayingAnimCallback = null;
        public PlayFrameCallback playFrameCallback = null;

        //----------------------
        // Private Variables

        // animator using this animation?
        private JCS_2DAnimator mJCS2DAnimator = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables Variables (JCS_I2DAnimator) **")]

        [SerializeField]
        private bool mTestWithKey = false;
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
        private bool mIsDonePlaying = false;


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

        [Tooltip("Null sprite.")]
        [SerializeField]
        private Sprite mNullSprite = null;


        [Tooltip("FPS for the animation to play.")]
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
        public int CurrentPlayingFrame {
            get { return this.mCurrentPlayingFrame; }
            set
            {
                this.mCurrentPlayingFrame = value;

                // make sure animation in the valid frame.
                PutAnimInFrame();
            }
        }
        public bool Loop { get { return this.mLoop; } set { this.mLoop = value; } }
        // Is the animation done playing?
        public bool IsDonePlaying { get { return this.mIsDonePlaying; } }
        public void SetJCS2DAnimator(JCS_2DAnimator jcs2dAnimator) { this.mJCS2DAnimator = jcs2dAnimator; }
        public float AnimationTimeProduction { get { return this.mAnimationTimeProduction; } }
        public Sprite CurrentSprite { get { return this.mAnimFrames[mCurrentPlayingFrame]; } }
        public Sprite NullSprite { get { return this.mNullSprite; } set { this.mNullSprite = value; } }

        // Bind.
        public void SetAnimationFrame(Sprite[] frames)
        {
            this.mAnimFrames = frames;
            Awake();
        }
        public Sprite[] GetAnimationFrame() { return this.mAnimFrames; }
        public float FramePerSec { get { return this.mFramePerSec; } set { this.mFramePerSec = value; } }
        

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();

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
            mIsDonePlaying = false;

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
            this.LocalSprite = this.mNullSprite;

            // stop the animation.
            mActive = false;
        }

        /// <summary>
        /// Default, use current playing frame index.
        /// </summary>
        public void PlayFrame()
        {
            PlayFrame(mCurrentPlayingFrame);
        }

        /// <summary>
        /// set the frame by index.
        /// </summary>
        public void PlayFrame(int frame)
        {
            this.mCurrentPlayingFrame = frame;

            PutAnimInFrame();

            Sprite currentPlayingSprite = mAnimFrames[this.mCurrentPlayingFrame];

            // set the current sprite.
            LocalSprite = currentPlayingSprite;

            // callback..
            if (playFrameCallback != null)
                playFrameCallback.Invoke();
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
                mIsDonePlaying = true;

                // do done play call back.
                if (donePlayingAnimCallback != null)
                    donePlayingAnimCallback.Invoke();
            }

            // set the current frame.
            PlayFrame(mCurrentPlayingFrame);

            // reset timer.
            mFrameTimer = 0;
        }

    }
}
