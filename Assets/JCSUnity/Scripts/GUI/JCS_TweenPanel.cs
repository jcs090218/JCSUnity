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
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_TweenPanel) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [SerializeField]
        private KeyCode mActiveKey = KeyCode.K;

        [SerializeField]
        private KeyCode mDeactiveKey = KeyCode.L;
#endif

        [Header("** Check Variables (JCS_TweenPanel) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Tooltip("Record down the starting value, in order to go back.")]
        [SerializeField]
        private Vector3 mStartingValue = Vector3.zero;

        [Header("** Runtime Variables (JCS_TweenPanel) **")]

        [Tooltip("Is panel active/tweened?")]
        [SerializeField]
        private bool mIsActive = false;

        [Tooltip("Do the tween effect to this value.")]
        [SerializeField]
        private Vector3 mTargetValue = Vector3.zero;

        [Header("** Sound Setttings (JCS_TweenPanel) **")]

        [Tooltip("Sound plays when active this panel.")]
        [SerializeField]
        private AudioClip mActiveSound = null;

        [Tooltip("Sound plays when this panel is deactive.")]
        [SerializeField]
        private AudioClip mDeactiveSound = null;

        private JCS_SoundPlayer mSoundPlayer = null;

        // call backs
        private ActiveCallback mActiveCallbackFunc = null;
        private DeactiveCallback mDeactiveCallbackFunc = null;

        /* Setter & Getter */

        public bool IsActive { get { return this.mIsActive; } }
        public JCS_TransformTweener TransformTweener { get { return this.mTransformTweener; } }
        public ActiveCallback ActiveCallbackFunc { get { return this.mActiveCallbackFunc; } set { this.mActiveCallbackFunc = value; } }
        public DeactiveCallback DeactiveCallbackFunc { get { return this.mDeactiveCallbackFunc; } set { this.mDeactiveCallbackFunc = value; } }

        /* Functions */

        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        private void Start()
        {
            switch (mTransformTweener.TweenType)
            {
                case JCS_TransformType.POSITION:
                    this.mStartingValue = this.mTransformTweener.LocalPosition;
                    break;
                case JCS_TransformType.ROTATION:
                    this.mStartingValue = this.mTransformTweener.LocalEulerAngles;
                    break;
                case JCS_TransformType.SCALE:
                    this.mStartingValue = this.mTransformTweener.LocalScale;
                    break;
            }

            // NOTE: Make compatible to resizable screen.
            {
                this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();
                if (mPanelRoot != null)
                {
                    mTargetValue.x /= mPanelRoot.PanelDeltaWidthRatio;
                    mTargetValue.y /= mPanelRoot.PanelDeltaHeightRatio;
                }
            }
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mActiveKey))
                Active();
            if (JCS_Input.GetKeyDown(mDeactiveKey))
                Deactive();
        }
#endif

        /// <summary>
        /// Tween to the taget position and play sound effect.
        /// </summary>
        public void Active()
        {
            if (!mTransformTweener.IsDoneTweening)
                return;

            mTransformTweener.DoTween(mTargetValue);
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

            mTransformTweener.DoTween(mStartingValue);
            mSoundPlayer.PlayOneShotWhileNotPlaying(mDeactiveSound);

            if (mDeactiveCallbackFunc != null)
                mDeactiveCallbackFunc.Invoke();

            this.mIsActive = false;
        }
    }
}
