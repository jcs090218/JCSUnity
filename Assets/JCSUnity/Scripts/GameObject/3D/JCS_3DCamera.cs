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

    [RequireComponent(typeof(AudioListener))]
    public class JCS_3DCamera 
        : JCS_Camera
    {


        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_3DCamera) **")]
        [SerializeField] private Transform mRotateTarget = null;
        [SerializeField] private float mRotateSpeed = 10.0f;

        private bool mDoReset = false;
        [SerializeField] private float mTargetAngle = 0.0f;
        [SerializeField] private float mAcceptRange = 1.0f;

        [Header("** Reset Camera Settings (JCS_3DCamera) **")]
        [SerializeField] private KeyCode mResetKeyCode = KeyCode.None;
        [SerializeField] private float mResetTargetAngle = 0;
        enum CheckState
        {
            NULL,

            POSITIVE,
            NEGATIVE
        };
        private CheckState mCheckState = CheckState.NULL;
        [SerializeField] private float testInput = 180;

        //-- Follow Object with frame distance
        private Vector3 mLastFramePos = Vector3.zero;

        [Header("** Up Down Settings (JCS_3DCamera) **")]
        [SerializeField] private bool mUpDownMovement = true;
        [SerializeField] private KeyCode mUpKey = KeyCode.None;
        [SerializeField] private KeyCode mDownKey = KeyCode.None;
        [SerializeField] private float mUpDownSpacing = 10;
        [Header("- Min / Max")]
        [Tooltip("How high the camera can reach.")]
        [SerializeField] private float mMaxHeight = 10;
        [Tooltip("How low the camera can reach.")]
        [SerializeField] private float mMinHeight = 0;
        private float mTargetHeight = 0;
        [Header("- Speed/Friction")]
        [SerializeField] private float mUpDownFriction = 0.2f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetRotateTarget(Transform targ) { this.mRotateTarget = targ; }
        public void SetRotateSpeed(float val) { this.mRotateSpeed = val; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            mLastFramePos = mRotateTarget.transform.position;

            // record down the height
            mTargetHeight = this.transform.position.y;
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            if (mRotateTarget == null)
                return;

            // update the angle from target we are following
            mTargetAngle = mRotateTarget.localEulerAngles.y;


            // follow the object with frame distance.
            // distance = current frame position - last frame position
            Vector3 currentFramePos = mRotateTarget.position;
            this.transform.position += currentFramePos - mLastFramePos;
            mLastFramePos = currentFramePos;

            DoResetToAxis();

            UpDownMovement();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            
#if (UNITY_EDITOR || UNITY_STANDALONE)
            if (JCS_Input.GetKeyDown(mResetKeyCode))
                ResetCamera();

            if (JCS_Input.GetKeyDown(mUpKey))
                UpCamera();
            else if (JCS_Input.GetKeyDown(mDownKey))
                DownCamera();
#endif
        }

        private void Test()
        {
            if (JCS_Input.GetKey(KeyCode.A))
            {
                RotateAroundRight();
            }
            if (JCS_Input.GetKey(KeyCode.D))
            {
                RotateAroundLeft();
            }
            if (JCS_Input.GetKey(KeyCode.R))
            {
                //ResetCamera();
                SetToRevoluationAngle(testInput);
            }

            
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        public void RotateAroundRight()
        {
            RotateAround(-mRotateSpeed);
        }
        public void RotateAroundLeft()
        {
            RotateAround(mRotateSpeed);
        }
        public void ResetCamera()
        {
            if (mRotateTarget == null)
            {
                JCS_GameErrors.JcsErrors("JCS_3DCamera",   "There is no target to reset the camera!");
                return;
            }

            float wantedDegree = mTargetAngle + 180;

            // set the degree with in 0-360 range to prevent 
            // out of range possiblity.
            float withIn360Degree = wantedDegree % 360;

            SetToRevoluationAngle(withIn360Degree);
        }

        public void UpCamera()
        {
            mTargetHeight += mUpDownSpacing;

            if (mMaxHeight < mTargetHeight)
                mTargetHeight = mMaxHeight;
        }
        public void DownCamera()
        {
            mTargetHeight -= mUpDownSpacing;

            if (mMinHeight > mTargetHeight)
                mTargetHeight = mMinHeight;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        private void RotateAround(float speed)
        {
            if (mRotateTarget == null)
            {
                JCS_GameErrors.JcsErrors("JCS_3DCamera",   "There is no target to reset the camera!");
                return;
            }

            this.transform.position = JCS_Mathf.RotatePointY(transform.position, Mathf.Cos(speed / 1000), Mathf.Sin(speed / 1000), mRotateTarget.position);
        }
        private void DoResetToAxis()
        {
            if (!mDoReset)
                return;

            Vector2 selfPos = new Vector2(
                this.transform.position.x,
                this.transform.position.z);
            Vector2 targetPos = new Vector2(
                mRotateTarget.position.x,
                mRotateTarget.position.z);

            // radius = distance between the two but not including y axis.
            float radius = Vector2.Distance(selfPos, targetPos);

            float diameter = radius * 2;

            float circumference = diameter * Mathf.PI;

            float currentAngle = this.transform.localEulerAngles.y;
            float circumferenceDistance = circumference * (mResetTargetAngle - currentAngle) / 360.0f;


            if (JCS_Mathf.isPositive(circumferenceDistance))
            {
                if (circumferenceDistance + mAcceptRange <= 0 ||
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
                if (circumferenceDistance + mAcceptRange >= 0 ||
                    mCheckState == CheckState.POSITIVE)
                {
                    mDoReset = false;
                    return;
                }

                RotateAroundLeft();

                mCheckState = CheckState.NEGATIVE;
            }
        }
        private void SetToRevoluationAngle(float angle)
        {
            if (mDoReset)
                return;

            mResetTargetAngle = angle;

            mDoReset = true;

            // reset check state
            mCheckState = CheckState.NULL;
        }
        private void UpDownMovement()
        {
            if (!mUpDownMovement)
                return;
            Vector3 newPos = this.transform.position;
            newPos.y += (mTargetHeight - newPos.y) / mUpDownFriction * Time.deltaTime;
            this.transform.position = newPos;
        }

    }
}

