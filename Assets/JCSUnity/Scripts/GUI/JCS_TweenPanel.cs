/**
 * $File: JCS_TweenPanel.cs $
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
    public delegate void ActiveCallback();
    public delegate void DeactiveCallback();

    /// <summary>
    /// Panel with active and deactive in order 
    /// to do back and forth effect.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_TweenPanel
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_TransformTweener mTransformTweener = null;


#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_TweenPanel) **")]

        [SerializeField]
        private bool mTestKey = true;

        [SerializeField]
        private KeyCode mActiveKey = KeyCode.K;

        [SerializeField]
        private KeyCode mDeactiveKey = KeyCode.L;
#endif


        [Header("** Check Variables (JCS_TweenPanel) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Tooltip("Record down the starting position, in order to go back.")]
        [SerializeField]
        private Vector3 mStartingPosition = Vector3.zero;


        [Header("** Runtime Variables (JCS_TweenPanel) **")]

        [Tooltip("Is panel active/tweened?")]
        [SerializeField]
        private bool mIsActive = false;

        [Tooltip("Do the tween effect to this position.")]
        [SerializeField]
        private Vector3 mTargetPosition = Vector3.zero;


        [Header("** Sound Setttings (JCS_TweenPanel) **")]

        [Tooltip("Sound play when active this panel.")]
        [SerializeField]
        private AudioClip mActiveSound = null;

        [Tooltip("Sound play when this panel is deactive.")]
        [SerializeField]
        private AudioClip mDeactiveSound = null;

        private JCS_SoundPlayer mSoundPlayer = null;

        // call backs
        private ActiveCallback mActiveCallbackFunc = null;
        private DeactiveCallback mDeactiveCallbackFunc = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsActive { get { return this.mIsActive; } }
        public JCS_TransformTweener TransformTweener { get { return this.mTransformTweener; } }
        public ActiveCallback ActiveCallbackFunc { get { return this.mActiveCallbackFunc; } set { this.mActiveCallbackFunc = value; } }
        public DeactiveCallback DeactiveCallbackFunc { get { return this.mDeactiveCallbackFunc; } set { this.mDeactiveCallbackFunc = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }
        private void Start()
        {
            this.mStartingPosition = this.mTransformTweener.LocalPosition;

            this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();
            if (mPanelRoot != null)
            {
                mStartingPosition.x /= mPanelRoot.PanelDeltaWidthRatio;
                mStartingPosition.y /= mPanelRoot.PanelDeltaHeightRatio;

                mTargetPosition.x /= mPanelRoot.PanelDeltaWidthRatio;
                mTargetPosition.y /= mPanelRoot.PanelDeltaHeightRatio;
            }
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (!mTestKey)
                return;

            if (JCS_Input.GetKeyDown(mActiveKey))
                Active();
            if (JCS_Input.GetKeyDown(mDeactiveKey))
                Deactive();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Tween to the taget position and play sound effect
        /// </summary>
        public void Active()
        {
            if (!mTransformTweener.IsDoneTweening)
                return;

            if (this.transform.localPosition == mTargetPosition)
                return;

            mTransformTweener.DoTween(mTargetPosition);
            mSoundPlayer.PlayOneShotWhileNotPlaying(mActiveSound);

            if (mActiveCallbackFunc != null)
                mActiveCallbackFunc.Invoke();

            this.mIsActive = true;
        }
        /// <summary>
        /// Tween back to the starting position and play sound effect.
        /// </summary>
        public void Deactive()
        {
            if (!mTransformTweener.IsDoneTweening)
                return;

            if (this.transform.localPosition == mStartingPosition)
                return;

            mTransformTweener.DoTween(mStartingPosition);
            mSoundPlayer.PlayOneShotWhileNotPlaying(mDeactiveSound);

            if (mDeactiveCallbackFunc != null)
                mDeactiveCallbackFunc.Invoke();

            this.mIsActive = false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
