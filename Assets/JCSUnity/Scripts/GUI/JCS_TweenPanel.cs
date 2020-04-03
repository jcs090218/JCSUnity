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
using System.Collections.Generic;

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
        /// <summary>
        /// Tweener information.
        /// 
        /// Contain these informations.
        ///   -> startingValue
        ///   -> transformTweener
        ///   -> targetValue
        /// </summary>
        [System.Serializable]
        public class JCS_TweenInfo
        {
            [Header("## Check Variables (JCS_TweenInfo)")]

            [Tooltip("Record down the starting value, in order to go back.")]
            public Vector3 startingValue = Vector3.zero;

            [Header("## Runtime Variables (JCS_TweenInfo)")]

            [Tooltip("Transform tweener we want to use.")]
            public JCS_TransformTweener transformTweener = null;

            [Tooltip("Do the tween effect to this value.")]
            public Vector3 targetValue = Vector3.zero;
        }

        /* Variables */

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

        [Header("** Runtime Variables (JCS_TweenPanel) **")]

        [Tooltip("Is panel active/tweened?")]
        [SerializeField]
        private bool mIsActive = false;

        [Tooltip("List of tweener informations.")]
        [SerializeField]
        private List<JCS_TweenInfo> mTweenInfos = null;

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
        public List<JCS_TweenInfo> TweenInfos { get { return this.mTweenInfos; } }
        public ActiveCallback ActiveCallbackFunc { get { return this.mActiveCallbackFunc; } set { this.mActiveCallbackFunc = value; } }
        public DeactiveCallback DeactiveCallbackFunc { get { return this.mDeactiveCallbackFunc; } set { this.mDeactiveCallbackFunc = value; } }

        /* Functions */

        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        private void Start()
        {
            // NOTE: Record down all the starting values.
            for (int index = 0; index < mTweenInfos.Count; ++index)
            {
                JCS_TweenInfo ti = mTweenInfos[index];
                switch (ti.transformTweener.TweenType)
                {
                    case JCS_TransformType.POSITION:
                        ti.startingValue = ti.transformTweener.LocalPosition;
                        break;
                    case JCS_TransformType.ROTATION:
                        ti.startingValue = ti.transformTweener.LocalEulerAngles;
                        break;
                    case JCS_TransformType.SCALE:
                        ti.startingValue = ti.transformTweener.LocalScale;
                        break;
                }
            }

            // NOTE: Make compatible to resizable screen.
            {
                this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();
                if (mPanelRoot != null)
                {
                    for (int index = 0; index < mTweenInfos.Count; ++index)
                    {
                        JCS_TweenInfo tt = mTweenInfos[index];
                        tt.targetValue.x /= mPanelRoot.PanelDeltaWidthRatio;
                        tt.targetValue.y /= mPanelRoot.PanelDeltaHeightRatio;
                    }
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
            if (!IsDoneTweening())
                return;

            DoTweenToTargetValue();
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
            if (!IsDoneTweening())
                return;

            DoTweenToStartValue();
            mSoundPlayer.PlayOneShotWhileNotPlaying(mDeactiveSound);

            if (mDeactiveCallbackFunc != null)
                mDeactiveCallbackFunc.Invoke();

            this.mIsActive = false;
        }

        /// <summary>
        /// Check if done tweeening for all transform tweeners.
        /// </summary>
        /// <returns>
        /// Return true, when is done with tweening.
        /// Return false, when is NOT done with tweening.
        /// </returns>
        private bool IsDoneTweening()
        {
            foreach (JCS_TweenInfo ti in mTweenInfos)
            {
                if (!ti.transformTweener.IsDoneTweening)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Make all transform tweeners tweene to the start value.
        /// </summary>
        private void DoTweenToStartValue()
        {
            foreach (JCS_TweenInfo ti in mTweenInfos)
                ti.transformTweener.DoTween(ti.startingValue);
        }

        /// <summary>
        /// Make all transform tweeners tweene to the target value.
        /// </summary>
        private void DoTweenToTargetValue()
        {
            foreach (JCS_TweenInfo ti in mTweenInfos)
                ti.transformTweener.DoTween(ti.targetValue);
        }
    }
}
