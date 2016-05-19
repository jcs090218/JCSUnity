/**
 * $File: JCS_2DTrackAction.cs $
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

    public class JCS_2DTrackAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [SerializeField] private bool mFollowing = true;
        [Tooltip("Transform we want to target")]
        [SerializeField] private Transform mTargetTransform = null;
        [Tooltip("Invers of Speed")]
        [SerializeField] private float mMoveFriction = 1;
        [Tooltip("Which plane we want to move")]
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_Z;

        private Vector3 mVelocity = Vector3.zero;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Following { get { return this.mFollowing; } set { this.mFollowing = value; } }
        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public void SetTargetTransform(Transform t) { this.mTargetTransform = t; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Update()
        {
            if (mFollowing)
                FollowObject();
            else
                KeepOnSameDirection();
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
        private void FollowObject()
        {

            if (mTargetTransform == null)
                return;

            Vector3 targetPos = mTargetTransform.position;
            Vector3 newPos = this.transform.position;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    mVelocity.x = 0;
                    mVelocity.y = (targetPos.y - newPos.y) / mMoveFriction * Time.deltaTime;
                    mVelocity.z = (targetPos.z - newPos.z) / mMoveFriction * Time.deltaTime;
                    break;
                case JCS_Axis.AXIS_Y:

                    mVelocity.x = (targetPos.x - newPos.x) / mMoveFriction * Time.deltaTime;
                    mVelocity.y = 0;
                    mVelocity.z = (targetPos.z - newPos.z) / mMoveFriction * Time.deltaTime;
                    break;
                case JCS_Axis.AXIS_Z:
                    mVelocity.x = (targetPos.x - newPos.x) / mMoveFriction * Time.deltaTime;
                    mVelocity.y = (targetPos.y - newPos.y) / mMoveFriction * Time.deltaTime;
                    mVelocity.z = 0;
                    break;
            }

           // apply force
            newPos += mVelocity;

            this.transform.position = newPos;
        }

        private void KeepOnSameDirection()
        {
            this.transform.position += mVelocity;
        }

    }
}
