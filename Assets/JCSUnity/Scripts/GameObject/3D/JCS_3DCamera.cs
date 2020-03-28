/**
 * $File: JCS_3DCamera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace JCSUnity
{
    /// <summary>
    /// Basic camera in 3d game.
    /// </summary>
    [RequireComponent(typeof(AudioListener))]
    public class JCS_3DCamera
        : JCS_Camera
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_3DCamera) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;
#endif


        [Header("** Check Variables (JCS_3DCamera) **")]

        [Tooltip("")]
        [SerializeField]
        private Vector3 mTrackPosition = Vector3.zero;


        [Header("** Runtime Variables (JCS_3DCamera) **")]

        [Tooltip("Target we want to look at.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        private bool mDoReset = false;

        [Tooltip("Angle when looking at the target.")]
        [SerializeField]
        private float mTargetAngle = 0.0f;

        [Tooltip("Do the smooth track instead of hard track.")]
        [SerializeField]
        private bool mSmoothTrack = false;

        [Tooltip("How fast the camera track in each axis?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mSmoothTrackFriction = 0.2f;


        [Header("** Rotate Camera Settings (JCS_3DCamera) **")]

        [Tooltip("How fast this camera rotates.")]
        [SerializeField]
        [Range(0.1f, 1000.0f)]
        private float mRotateSpeed = 350.0f;

        [Tooltip("Range when rotating to the target rotation.")]
        [SerializeField]
        [Range(0.01f, 2.0f)]
        private float mAcceptRange = 1.0f;

#if (UNITY_EDITOR || UNITY_STANDALONE)
        [Tooltip("Key to rotate around the left side.")]
        [SerializeField]
        private KeyCode mRotateAroundLeft = KeyCode.None;

        [Tooltip("Key to rotate around the right side.")]
        [SerializeField]
        private KeyCode mRotateAroundRight = KeyCode.None;


        [Header("** Reset Camera Settings (JCS_3DCamera) **")]

        [Tooltip("Key to reset the camera.")]
        [SerializeField]
        private KeyCode mResetKeyCode = KeyCode.None;

#endif

        [Tooltip("Angle when reset the camera.")]
        [SerializeField]
        private float mResetTargetAngle = 0.0f;

        /// <summary>
        /// State of the rotation identifier.
        /// </summary>
        enum CheckState
        {
            NULL,

            POSITIVE,
            NEGATIVE
        };
        // instance rotation state.
        private CheckState mCheckState = CheckState.NULL;


        //-- Follow Object with frame distance
        private Vector3 mLastFramePos = Vector3.zero;


        [Header("** Up Down Settings (JCS_3DCamera) **")]

        [Tooltip("Up and Down movement enable?")]
        [SerializeField]
        private bool mUpDownMovement = true;

#if (UNITY_EDITOR || UNITY_STANDALONE)
        [Tooltip("Key to go up.")]
        [SerializeField]
        private KeyCode mUpKey = KeyCode.None;

        [Tooltip("Key to go down.")]
        [SerializeField]
        private KeyCode mDownKey = KeyCode.None;
#endif

        [Tooltip("Space between each up and down movement.")]
        [SerializeField]
        private float mUpDownSpacing = 10;


        [Header("- Min / Max")]

        [Tooltip("How high the camera can reach.")]
        [SerializeField]
        private float mMaxHeight = 10;

        [Tooltip("How low the camera can reach.")]
        [SerializeField]
        private float mMinHeight = 0;

        // 
        private float mTargetHeight = 0;

        [Header("- Speed/Friction")]

        [Tooltip("How fast it change the view up and down?")]
        [SerializeField]
        private float mUpDownFriction = 0.2f;

        [Header("** Scroll / Zoom Settings **")]

        [Tooltip("Do the zoom effect?")]
        [SerializeField]
        private bool mZoomEffect = true;

        [Tooltip("Zoom with the mouse or touches.")]
        [SerializeField]
        private bool mZoomWithMouseOrTouch = true;

        [Tooltip("Distance once you scroll.")]
        [SerializeField]
        [Range(0.0f, 500.0f)]
        private float mScrollRange = 120.0f;

        [Tooltip("How fast it scroll speed get reduce?")]
        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float mScrollSpeedFriction = 0.4f;

        // the real target of the speed
        private float mTargetScrollSpeed = 0.0f;

        // Record the position down for zoom effect.
        private Vector3 mRecordPosition = Vector3.zero;

        // scroll distance.
        private float mWheelDegree = 0.0f;


        [Header("- Min / Max")]

        [Tooltip("Mininum distance camera can approach to?")]
        [SerializeField]
        [Range(0.01f, 500.0f)]
        private float mMinDistance = 2.0f;

        [Tooltip("Maxinum distance camera can far away from?")]
        [SerializeField]
        [Range(10.0f, 1000.0f)]
        private float mMaxDistance = 200.0f;


        /* Setter & Getter */

        public Transform FollowTarget { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public override void SetFollowTarget(Transform targ) { this.mTargetTransform = targ; }
        public override Transform GetFollowTarget() { return this.mTargetTransform; }

        public void SetRotateSpeed(float val) { this.mRotateSpeed = val; }
        public bool SmoothTrack
        {
            get { return this.mSmoothTrack; }
            set
            {
                this.mSmoothTrack = value;

                // record down the position if enabled.
                mTrackPosition = this.transform.position;
            }
        }
        public bool ZoomEffect { get { return this.mZoomEffect; } set { this.mZoomEffect = value; } }
        public bool ZoomWithMouseOrTouch { get { return this.mZoomWithMouseOrTouch; } set { this.mZoomWithMouseOrTouch = value; } }

        /* Functions */

        protected override void Start()
        {
            base.Start();

            if (mTargetTransform == null)
            {
                JCS_Debug.LogError(
                    "Cannot record the frame without the target transform...");

                return;
            }

            mLastFramePos = mTargetTransform.position;

            // record down the height
            mTargetHeight = this.transform.position.y;

            // record down the transform position.
            mTrackPosition = this.transform.position;
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            if (mTargetTransform == null)
                return;

            InputCamera();

            // update the angle from target we are following
            mTargetAngle = mTargetTransform.localEulerAngles.y;


            DoFollowing();

            DoResetToAxis();

            UpDownMovement();


            this.mRecordPosition = this.transform.position;

            if (mZoomWithMouseOrTouch)
            {
#if (UNITY_EDITOR || UNITY_STANDALONE)
                // Get the wheel value from the Unity API
                // 
                // physical layer [mouse wheel] ->
                // OS layer ->
                // application layer [Unity itself] ->
                // to here...
                mWheelDegree = Input.GetAxis("Mouse ScrollWheel");
#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
                JCS_SlideInput slideInput = JCS_InputManager.instance.GetGlobalSlideInput();
                mWheelDegree = slideInput.TouchDistance;
#endif
                ZoomCamera(mWheelDegree);
            }

            Vector3 newPos = Vector3.forward * mTargetScrollSpeed * Time.deltaTime;

            // if is valid, do action.
            if (!JCS_Mathf.IsNaN(newPos))
            {
                // record down the position before calling the
                // translate function
                Vector3 currentPos = this.transform.position;

                // do translate base on the scrolling distance
                // we get from the input buffer.
                this.transform.Translate(newPos);

                // get the updated position!
                Vector3 afterTransPos = this.transform.position;

                // update the track position
                mTrackPosition += afterTransPos - currentPos;
            }

            // Fix the speed if reach the distance!
            FixedMinMaxDistance();

            // asymptotic back to zero
            mTargetScrollSpeed += (0.0f - mTargetScrollSpeed) / mScrollSpeedFriction * Time.deltaTime;
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(KeyCode.N))
            {
                SmoothTrack = !SmoothTrack;
            }
        }
#endif

        /// <summary>
        /// Rotate around the target toward right.
        /// </summary>
        public void RotateAroundRight()
        {
            RotateAround(-mRotateSpeed);
        }

        /// <summary>
        /// Rotate around the target toward left.
        /// </summary>
        public void RotateAroundLeft()
        {
            RotateAround(mRotateSpeed);
        }

        /// <summary>
        /// Reset the camera.
        /// </summary>
        public void ResetCamera()
        {
            if (mTargetTransform == null)
            {
                JCS_Debug.LogError("There is no target to reset the camera!");
                return;
            }

            float wantedDegree = mTargetAngle + 180.0f;

            // set the degree with in 0-360 range to prevent 
            // out of range possiblity.
            float withIn360Degree = wantedDegree % 360.0f;

            SetToRevoluationAngle(withIn360Degree);
        }

        /// <summary>
        /// Move the camera upward.
        /// </summary>
        public void UpCamera()
        {
            mTargetHeight += mUpDownSpacing;

            if (mMaxHeight < mTargetHeight)
                mTargetHeight = mMaxHeight;
        }

        /// <summary>
        /// Move the caemra downward.
        /// </summary>
        public void DownCamera()
        {
            mTargetHeight -= mUpDownSpacing;

            if (mMinHeight > mTargetHeight)
                mTargetHeight = mMinHeight;
        }

        /// <summary>
        /// Do the follow algorithm.
        /// </summary>
        private void DoFollowing()
        {
            if (!mFollowing)
                return;

            // Hard track
            if (!mSmoothTrack)
            {
                // follow the object with frame distance.
                // distance = current frame position - last frame position
                Vector3 currentFramePos = mTargetTransform.position;
                this.transform.position += currentFramePos - mLastFramePos;
                mLastFramePos = currentFramePos;
            }
            // Smooth track.
            else
            {
                // follow the object with frame distance.
                // distance = current frame position - last frame position
                // , but in smooth track, we set the track position instead 
                // of the real position.
                Vector3 currentFramePos = mTargetTransform.position;
                mTrackPosition += currentFramePos - mLastFramePos;
                mLastFramePos = currentFramePos;

                // update the transform position
                this.transform.position += (this.mTrackPosition - this.transform.position) / mSmoothTrackFriction * Time.deltaTime;
            }
        }

        /// <summary>
        /// Do the revolution rotate base on speed.
        /// </summary>
        /// <param name="speed"> direction and speed. </param>
        private void RotateAround(float speed)
        {
            if (mTargetTransform == null)
            {
                JCS_Debug.LogError("There is no target to reset the camera!");
                return;
            }

            // to the rotate formula.
            this.transform.position = JCS_Mathf.RotatePointY(transform.position, Mathf.Cos(speed / 1000.0f), Mathf.Sin(speed / 1000.0f), mTargetTransform.position);

            // NOTE(jenchieh): also set to the track position.
            mTrackPosition = this.transform.position;
        }

        /// <summary>
        /// Reset the camera to certain axis.
        /// </summary>
        private void DoResetToAxis()
        {
            if (!mDoReset)
                return;

            Vector2 selfPos = new Vector2(
                this.transform.position.x,
                this.transform.position.z);
            Vector2 targetPos = new Vector2(
                mTargetTransform.position.x,
                mTargetTransform.position.z);

            // radius = distance between the two but not including y axis.
            float radius = Vector2.Distance(selfPos, targetPos);

            float diameter = radius * 2;

            float circumference = diameter * Mathf.PI;

            float currentAngle = this.transform.localEulerAngles.y;
            float circumferenceDistance = circumference * (mResetTargetAngle - currentAngle) / 360.0f;


            if (JCS_Mathf.isPositive(circumferenceDistance))
            {
                if (circumferenceDistance + mAcceptRange <= 0.0f ||
                    mCheckState == CheckState.NEGATIVE)
                {
                    mDoReset = false;
                    return;
                }

                RotateAroundRight();

                mCheckState = CheckState.POSITIVE;
            }
            else
            {
                if (circumferenceDistance + mAcceptRange >= 0.0f ||
                    mCheckState == CheckState.POSITIVE)
                {
                    mDoReset = false;
                    return;
                }

                RotateAroundLeft();

                mCheckState = CheckState.NEGATIVE;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        private void SetToRevoluationAngle(float angle)
        {
            if (mDoReset)
                return;

            mResetTargetAngle = angle;

            mDoReset = true;

            // reset check state
            mCheckState = CheckState.NULL;
        }

        /// <summary>
        /// Up down movement algorithm.
        /// </summary>
        private void UpDownMovement()
        {
            // Check if the effect enabled.
            if (!mUpDownMovement)
                return;

            // get current position.
            Vector3 newPos = this.transform.position;

            {
                // do the delta position formula
                float deltaHeight = (mTargetHeight - newPos.y) / mUpDownFriction * Time.deltaTime;

                // apply to new position, so get ready to 
                // apply on the current position.
                newPos.y += deltaHeight;

                // NOTE(jenchieh): also apply to the smooth track.
                mTrackPosition.y += deltaHeight;
            }

            // apply new position
            this.transform.position = newPos;
        }

        /// <summary>
        /// Process the input of this camera.
        /// </summary>
        private void InputCamera()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)
            if (JCS_Input.GetKeyDown(mResetKeyCode))
                ResetCamera();

            if (JCS_Input.GetKeyDown(mUpKey))
                UpCamera();
            else if (JCS_Input.GetKeyDown(mDownKey))
                DownCamera();

            if (JCS_Input.GetKey(mRotateAroundLeft))
                RotateAroundLeft();
            else if (JCS_Input.GetKey(mRotateAroundRight))
                RotateAroundRight();
#endif
        }

        /// <summary>
        /// Do the scrolling algorithm.
        /// </summary>
        private void ZoomCamera(float depthDistance)
        {
            // check the trigger of the 
            // scrolling effect.
            if (!mZoomEffect)
                return;

            if (depthDistance == 0.0f)
                return;

            this.mTargetScrollSpeed = this.mWheelDegree * this.mScrollRange;
        }

        /// <summary>
        /// Fix the speed/range if reach the min/max distance.
        /// </summary>
        private void FixedMinMaxDistance()
        {
            float currentDistance = Vector3.Distance(this.transform.position, mTargetTransform.position);
            if (JCS_Mathf.isPositive(mTargetScrollSpeed))
            {
                if (currentDistance <= mMinDistance)
                {
                    mTargetScrollSpeed = 0.0f;

                    Vector3 newPos = this.transform.position;
                    newPos.z = mRecordPosition.z;
                    this.transform.position = newPos;
                }
            }
            else
            {
                if (currentDistance >= mMaxDistance)
                {
                    mTargetScrollSpeed = 0.0f;

                    Vector3 newPos = this.transform.position;
                    newPos.z = mRecordPosition.z;
                    this.transform.position = newPos;
                }
            }
        }
    }
}

