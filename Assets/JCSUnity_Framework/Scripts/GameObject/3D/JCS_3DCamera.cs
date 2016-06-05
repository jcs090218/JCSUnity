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

        [Header("** Runtime Variables **")]
        [SerializeField] private Transform mTarget = null;
        [SerializeField] private float mRadius = 2.0f;
        [SerializeField] private float mRadiusSpeed = 0.5f;
        [SerializeField] private float mRotateSpeed = 10.0f;

        private bool mDoReset = false;
        [SerializeField] private float mTargetAngle = 0.0f;
        [SerializeField] private float mFriction = 0.1f;
        [SerializeField] private float mAcceptedRange = 1.0f;

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

        }

        private void Update()
        {
            DoResetToAxis();

            // update the angle from target we are following
            if (mTarget != null)
                mTargetAngle = mTarget.localEulerAngles.y;
            


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
                ResetCamera();
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
            if (mTarget == null)
            {
                JCS_GameErrors.JcsErrors("JCS_3DCamera", -1, "There is no target to reset the camera!");
                return;
            }

            ResetCameraToAxis();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void RotateAround(float speed)
        {
            // if still doing the effect
            if (mDoReset)
                return;

            if (mTarget == null)
            {
                JCS_GameErrors.JcsErrors("JCS_3DCamera", -1, "There is no target to reset the camera!");
                return;
            }

            this.transform.RotateAround(mTarget.position, Vector3.up, speed * Time.deltaTime);

            Vector3 desiredPosition = (transform.position - mTarget.position).normalized * mRadius + mTarget.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * mRadiusSpeed);
        }
        private void ResetCameraToAxis()
        {
            if (mTargetAngle <= -1 || mTargetAngle >= 361)
            {
                JCS_GameErrors.JcsErrors("JCS_3DCamera", -1, "Target Angle should be within the Range of 0 ~ 360!");
                return;
            }

            mDoReset = true;
        }
        private void DoResetToAxis()
        {
            if (!mDoReset)
                return;

            Vector3 newRotation = this.transform.rotation.eulerAngles;

            if (newRotation.y + mAcceptedRange > mTargetAngle &&
                newRotation.y - mAcceptedRange < mTargetAngle)
            {
                mDoReset = false;
                return;
            }

            float speed = (mTargetAngle - newRotation.y) / mFriction;

            int direction = CheckRotateDirectionToTheClosest();

            if (direction == -1)
            {
                // please treat new rotation as current rotation here
                float diffAngle = 360 - newRotation.y;

                speed = (mTargetAngle - diffAngle) / mFriction;
            }

            speed *= direction;

            this.transform.RotateAround(mTarget.position, Vector3.up, speed * Time.deltaTime);
        }

        private int CheckRotateDirectionToTheClosest()
        {
            int direction = 1;

            Vector3 currentRotatetion = this.transform.rotation.eulerAngles;

            float middleOfTheAngle;

            if (mTargetAngle < 180)
                middleOfTheAngle = mTargetAngle + 180;
            else
                middleOfTheAngle = mTargetAngle - 180;

            if (currentRotatetion.y < middleOfTheAngle)
                return direction;
            else
                return -direction;
        }

    }
}

