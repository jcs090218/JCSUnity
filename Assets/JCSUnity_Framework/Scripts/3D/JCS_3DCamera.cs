/**
 * $File: JCS_3DCamera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace JCSUnity
{
    [RequireComponent(typeof(AudioListener))]
    public class JCS_3DCamera : JCS_Camera
    {


        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private Transform mTarget = null;
        [SerializeField] private float mRadius = 2.0f;
        [SerializeField] private float mRadiusSpeed = 0.5f;
        [SerializeField] private float mRotateSpeed = 10.0f;
        [SerializeField] private float mLockRotateSpeed = 300.0f;
        [SerializeField] private JCS_Axis mLockAxis = JCS_Axis.AXIS_X;
        private Vector3 mLockAxisVec = Vector3.one;

        [SerializeField] private float mRotatedAngle = 0.0f;
        private float mTargetRotateAngle = 0.0f;
        private bool mDoReset = false;
        private float mTargetAngle = 0.0f;
        private float mAcceptedRange = 1.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetTarget(Transform targ) { this.mTarget = targ; }
        public void SetRadius(float val) { this.mRadius = val; }
        public void SetRadiusSpeed(float val) { this.mRadiusSpeed = val; }
        public void SetRotateSpeed(float val) { this.mRotateSpeed = val; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            transform.position = (transform.position - mTarget.position).normalized * mRadius + mTarget.position;

            switch (mLockAxis)
            {
                case JCS_Axis.AXIS_X:
                    break;
                case JCS_Axis.AXIS_Y:
                    mLockAxisVec = Vector3.up;
                    break;
                case JCS_Axis.AXIS_Z:
                    break;
            }
        }

        private void Update()
        {
            DoResetToAxis();

            if (JCS_Input.GetKey(KeyCode.A))
            {
                RotateAroundRight();
            }
            if (JCS_Input.GetKey(KeyCode.D))
            {
                RotateAroundLeft();
            }
            if (JCS_Input.GetKey(KeyCode.L))
            {
                ResetCamera();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public override void TakeScreenShot()
        {

        }

        public void RotateAroundRight()
        {
            RotateAround(mRotateSpeed);
        }
        public void RotateAroundLeft()
        {
            RotateAround(-mRotateSpeed);
        }
        public void ResetCamera()
        {
            ResetCameraToAxis();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void RotateAround(float speed)
        {
            this.transform.RotateAround(mTarget.position, Vector3.up, speed * Time.deltaTime);

            Vector3 desiredPosition = (transform.position - mTarget.position).normalized * mRadius + mTarget.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * mRadiusSpeed);
        }
        private void ResetCameraToAxis()
        {
            Vector3 camRotation = this.transform.rotation.eulerAngles;
            Vector3 targetRotation = mTarget.transform.rotation.eulerAngles;

            mTargetRotateAngle = camRotation.y - targetRotation.y;

            // set the angle to absolute value
            mTargetRotateAngle = JCS_Mathf.AbsoluteValue(mTargetRotateAngle);

            if (camRotation.y < targetRotation.y)
            {
                if (camRotation.y - mAcceptedRange > mTargetRotateAngle)
                    return;
                mLockRotateSpeed = JCS_Mathf.ToPositive(mLockRotateSpeed);
            }
            else if (camRotation.y > targetRotation.y)
            {
                if (camRotation.y + mAcceptedRange < mTargetRotateAngle)
                    return;
                mLockRotateSpeed = JCS_Mathf.ToNegative(mLockRotateSpeed);
            }

            mDoReset = true;
            
        }
        private void DoResetToAxis()
        {
            if (!mDoReset)
                return;

            float anglePerSecond = mLockRotateSpeed * Time.deltaTime;
            

            bool achieve = false;

            if (JCS_Mathf.isPositive(mLockRotateSpeed))
            {
                if (mRotatedAngle + mAcceptedRange > mTargetRotateAngle)
                {
                    achieve = true;
                }
            }
            else if (JCS_Mathf.isNegative(mLockRotateSpeed))
            {
                if (mRotatedAngle - mAcceptedRange < mTargetRotateAngle)
                {
                    achieve = true;
                }
            }
            else
            {
                achieve = true;
            }

            if (achieve)
            {
                mRotatedAngle = 0;
                mDoReset = false;
                return;
            }

            // record the turned angle per frame, do not forget to reset back
            // to zero after camera have been reset
            this.mRotatedAngle += anglePerSecond;

            // do rotation
            this.transform.RotateAround(mTarget.position, mLockAxisVec, anglePerSecond);
        }

    }
}

