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
    [RequireComponent(typeof(JCS_TransfromTweener))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_TweenPanel
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private JCS_TransfromTweener mTweener = null;


#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_TweenPanel) **")]

        [SerializeField]
        private bool mTestKey = true;
#endif

        [Header("** Check Variables (JCS_TweenPanel) **")]

        [Tooltip("Do the tween effect to this position.")]
        [SerializeField]
        private Vector3 mTargetPosition = Vector3.zero;

        [Tooltip("")]
        [SerializeField]
        // record down the starting position, in order to go back.
        private Vector3 mStartingPosition = Vector3.zero;


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
        public ActiveCallback ActiveCallbackFunc { get { return this.mActiveCallbackFunc; } set { this.mActiveCallbackFunc = value; } }
        public DeactiveCallback DeactiveCallbackFunc { get { return this.mDeactiveCallbackFunc; } set { this.mDeactiveCallbackFunc = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mTweener = this.GetComponent<JCS_TransfromTweener>();
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }
        private void Start()
        {
            mStartingPosition = this.mTweener.LocalPosition;
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (!mTestKey)
                return;

            if (JCS_Input.GetKeyDown(KeyCode.I))
                Active();
            if (JCS_Input.GetKeyDown(KeyCode.O))
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
            mTweener.DoTween(mTargetPosition);
            mSoundPlayer.PlayOneShotWhileNotPlaying(mActiveSound);

            if (mActiveCallbackFunc != null)
                mActiveCallbackFunc.Invoke();
        }
        /// <summary>
        /// Tween back to the starting position and play sound effect.
        /// </summary>
        public void Deactive()
        {
            mTweener.DoTween(mStartingPosition);
            mSoundPlayer.PlayOneShotWhileNotPlaying(mDeactiveSound);

            if (mDeactiveCallbackFunc != null)
                mDeactiveCallbackFunc.Invoke();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
