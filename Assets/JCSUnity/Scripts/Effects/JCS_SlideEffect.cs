/**
 * $File: JCS_SlideEffect.cs $
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

    /// <summary>
    /// Make object to make the smooth slide effect.
    ///     - Could be compose with JCS_TransfromTweener class.
    /// </summary>
    public class JCS_SlideEffect
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables (JCS_SlideEffect) **")]

        [SerializeField]
        private bool mIsActive = false;


        [Header("** Initialize Variables (JCS_SlideEffect) **")]

        [Tooltip("Direction object slides.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        [Tooltip("How far the object slides.")]
        [SerializeField] [Range(-30000, 30000)]
        private float mDistance = -50;

        [Tooltip("Check if the mouse leave the button or not to disable the slide effect.")]
        [SerializeField]
        private bool mAutoCheckExit = true;

        [Tooltip("How fast the object slides.")]
        [SerializeField] [Range(0.01f, 10.0f)]
        private float mFriction = 0.2f;

        private Vector3 mTargetPosition = Vector3.zero;

        private Vector3 mRecordPosition = Vector3.zero;
        private Vector3 mTowardPosition = Vector3.zero;


        [Header("Usage:(Audio) Add JCS_SoundPlayer components, if u want the SFX.")]

        [Tooltip("If slide out, do the sound.")]
        [SerializeField]
        private AudioClip mActiveClip = null;

        [Tooltip("If slide back the original position, do the sound.")]
        [SerializeField]
        private AudioClip mDeactiveClip = null;

        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Don't track on x-axis?")]
        [SerializeField]
        private bool mIgnoreX = false;

        [Tooltip("Don't track on y-axis?")]
        [SerializeField]
        private bool mIgnoreY = false;

        [Tooltip("Don't track on z-axis?")]
        [SerializeField]
        private bool mIgnoreZ = false;


        [Header("** Optional Variables (JCS_SlideEffect) **")]

        [Tooltip(@"If u want to active this effect by button, 
plz set the button here.")]
        [SerializeField]
        private JCS_Button mActiveButton = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsActive { get { return this.mIsActive; } }
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }
        public bool AutoCheckExit { get { return this.mAutoCheckExit; } set { this.mAutoCheckExit = value; } }
        public void SetActiveSound(AudioClip ac) { this.mActiveClip = ac; }
        public void SetDeactiveSound(AudioClip ac) { this.mDeactiveClip = ac; }
        public float Friction { get { return this.mFriction; } set { this.mFriction = value; } }
        public float Distance
        {
            get { return this.mDistance; }
            set
            {
                this.mDistance = value;

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
        }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // JCS_SoundPlayer will be optional.
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            UpdateUnityData();

            // set the call back function if there is button assigned.
            if (mActiveButton != null)
                mActiveButton.SetSystemCallback(JCS_OnMouseOver);


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

        /// <summary>
        /// Use for inspector. (Active)
        /// </summary>
        public void JCS_OnMouseOver()
        {
            Active();
        }

        /// <summary>
        /// Use for inspector. (Deactive)
        /// </summary>
        /// <returns></returns>
        public bool JCS_OnMouseExit()
        {
            if (!mIsActive)
                return false;

            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_Utility.MouseOverGUI(this.mRectTransform))
                    return false;
            }

            Deactive();

            return true;
        }

        /// <summary>
        /// Active the effect. (Script)
        /// </summary>
        public void Active()
        {
            mIsActive = true;
            mTargetPosition = mTowardPosition;

            if (mSoundPlayer != null)
                mSoundPlayer.PlayOneShot(mActiveClip);
        }

        /// <summary>
        /// Deactive the effect. (Script)
        /// </summary>
        public void Deactive()
        {
            mIsActive = false;
            mTargetPosition = mRecordPosition;

            if (mSoundPlayer != null)
                mSoundPlayer.PlayOneShot(mDeactiveClip);
        }


        /// <summary>
        /// Check the object in at the position.
        /// </summary>
        /// <param name="accept"> acceptable range </param>
        /// <returns> 
        /// true: at the position, 
        /// false: not at the position 
        /// </returns>
        public bool IsIdle(float accept = 0)
        {
            int distance = (int)Vector3.Distance(mTargetPosition, this.transform.localPosition);

            if (accept == 0)
                return (distance == 0);

            return (distance < accept);
        }

        /// <summary>
        /// Check the mouse if over the panel.
        /// </summary>
        /// <returns> true: is over, false: not over </returns>
        public bool IsOnThere()
        {
            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_Utility.MouseOverGUI(this.mRectTransform))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Check the mouse if over the panel.
        /// </summary>
        /// <param name="rootPanel"> if there are root child plz use this to get the correct calculation </param>
        /// <returns> true: is over, false: not over </returns>
        public bool IsOnThere(RectTransform rootPanel)
        {
            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                if (JCS_Utility.MouseOverGUI(this.mRectTransform, rootPanel))
                    return true;
            }

            return false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Main algorithm to do the slide effect.
        /// </summary>
        private void SlideEffect()
        {
            Vector3 tempTargetPost = mTargetPosition;

            if (mIgnoreX)
                tempTargetPost.x = this.LocalPosition.x;
            if (mIgnoreY)
                tempTargetPost.y = this.LocalPosition.y;
            if (mIgnoreZ)
                tempTargetPost.z = this.LocalPosition.z;

            LocalPosition += (tempTargetPost - LocalPosition) / mFriction * Time.deltaTime;
        }

    }
}
