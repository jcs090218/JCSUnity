/**
 * $File: JCS_SlideEffect.cs $
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

    /// <summary>
    /// Make object to make the smooth slide effect.
    ///     - Could be compose with JCS_Tweener class.
    /// </summary>
    public class JCS_SlideEffect
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables **")]
        [SerializeField] private bool mIsActive = false;

        [Header("** Initialize Variables **")]
        [Tooltip("Direction object slides.")]
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_X;
        [Tooltip("How far the object slides.")]
        [SerializeField] private float mDistance = 10;

        [Tooltip("Check if the mouse leave the button or not to disable the slide effect.")]
        [SerializeField] private bool mAutoCheckExit = true;

        [Tooltip("How fast the object slides.")]
        [SerializeField] private float mFriection = 12;
        private Vector3 mTargetPosition = Vector3.zero;

        private Vector3 mRecordPosition = Vector3.zero;
        private Vector3 mTowardPosition = Vector3.zero;

        [Header("Usage:(Audio) Add JCS_SoundPlayer components, if u want the SFX.")]
        [SerializeField] private AudioClip mActiveClip = null;
        [SerializeField] private AudioClip mDeactiveClip = null;
        private JCS_SoundPlayer mSoundPlayer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsActive { get { return this.mIsActive; } }
        public bool AutoCheckExit { get { return this.mAutoCheckExit; } set { this.mAutoCheckExit = value; } }
        public void SetActiveSound(AudioClip ac) { this.mActiveClip = ac; }
        public void SetDeactiveSound(AudioClip ac) { this.mDeactiveClip = ac; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // JCS_SoundPlayer will be optional.
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            UpdateUnityData();
        }
        private void Start()
        {
            Vector3 newPos = this.transform.localPosition;
            // record the original position
            this.mRecordPosition = newPos;
            this.mTargetPosition = newPos;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    newPos.x += mDistance;
                    break;
                case JCS_Axis.AXIS_Y:
                    newPos.y += mDistance;
                    break;
                case JCS_Axis.AXIS_Z:
                    newPos.z += mDistance;
                    break;
            }

            this.mTowardPosition = newPos;
        }

        private void Update()
        {
            SlideEffect();

            if (mAutoCheckExit)
                JCS_OnMouseExit();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void JCS_OnMouseOver()
        {
            Active();
        }
        public bool JCS_OnMouseExit()
        {
            if (!mIsActive)
                return false;

            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_UsefualFunctions.MouseOverGUI(this.mRectTransform))
                    return false;
            }

            Deactive();

            return true;
        }

        public void Active()
        {
            mIsActive = true;
            mTargetPosition = mTowardPosition;

            if (mSoundPlayer != null)
                mSoundPlayer.PlayOneShot(mActiveClip);
        }
        public void Deactive()
        {
            mIsActive = false;
            mTargetPosition = mRecordPosition;

            if (mSoundPlayer != null)
                mSoundPlayer.PlayOneShot(mDeactiveClip);
        }
        public bool IsIdle()
        {
            int distance = (int)Vector3.Distance(mTargetPosition, this.transform.localPosition);

            return (distance == 0);
        }
        public bool IsOnThere()
        {
            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_UsefualFunctions.MouseOverGUI(this.mRectTransform))
                    return true;
            }

            return false;
        }
        public bool IsOnThere(RectTransform rootPanel)
        {
            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_UsefualFunctions.MouseOverGUI(this.mRectTransform, rootPanel))
                    return true;
            }

            return false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void SlideEffect()
        {
            this.transform.localPosition += (mTargetPosition - transform.localPosition) / mFriection * Time.deltaTime;
        }

    }
}
