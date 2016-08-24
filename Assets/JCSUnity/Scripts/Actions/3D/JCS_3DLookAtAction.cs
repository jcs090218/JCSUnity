/**
 * $File: JCS_3DLookAtAction.cs $
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
    /// 
    /// </summary>
    public class JCS_3DLookAtAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        public enum State
        {
            POSITIVE = 1,
            NEGATIVE = -1
        };

        //----------------------
        // Private Variables
        [SerializeField] private Transform mTargetTransform = null;
        [SerializeField] private JCS_Vector3Direction mLookDirection = JCS_Vector3Direction.UP;

        [SerializeField] private State mState = State.POSITIVE;

        [Tooltip("")]
        [SerializeField]
        private bool mRotateBack90 = false;

        [SerializeField]
        private bool mFreezeX = false;
        [SerializeField]
        private bool mFreezeY = false;
        [SerializeField]
        private bool mFreezeZ = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public void SetTargetTransform(Transform t) { this.mTargetTransform = t; }
        public bool RotateBack90 { get { return this.mRotateBack90; } set { this.mRotateBack90 = value; } }

        //========================================
        //      Unity's function
        //------------------------------

        private void LateUpdate()
        {
            LookAt();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void LookAt()
        {
            if (mTargetTransform == null)
                return;

            Vector3 lookPoint = mTargetTransform.position;

            Vector3 direction = Vector3.up;

            switch (mLookDirection)
            {
                case JCS_Vector3Direction.UP:
                    direction = Vector3.up;
                    break;
                case JCS_Vector3Direction.DOWN:
                    direction = Vector3.down;
                    break;

                case JCS_Vector3Direction.FORWARD:
                    direction = Vector3.forward;
                    break;
                case JCS_Vector3Direction.BACK:
                    direction = Vector3.back;
                    break;

                case JCS_Vector3Direction.RIGHT:
                    direction = Vector3.right;
                    break;
                case JCS_Vector3Direction.LEFT:
                    direction = Vector3.left;
                    break;
            }

            if (mFreezeX)
                lookPoint.x = this.transform.position.x;

            if (mFreezeY)
                lookPoint.y = this.transform.position.y;

            if (mFreezeZ)
                lookPoint.z = this.transform.position.z;

            transform.LookAt(lookPoint, direction * (int)mState);

            // TODO(JenChieh): study the rotation going on in 
            //                Unity lower level archietecture.
            // rotate back to X-axis.
            if (mRotateBack90)
                transform.Rotate(0, -90, 0);
        }

    }
}
