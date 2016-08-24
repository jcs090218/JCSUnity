/**
 * $File: JCS_2DTrackAction.cs $
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
    public class JCS_2DTrackAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Check Variables **")]
        [SerializeField] private float mIndex = 0;
        [SerializeField] private int mOrderIndex = 0;
        [SerializeField] private bool mFollowing = true;

        [Header("** Runtime Variables **")]
        [Tooltip("Transform we want to target")]
        [SerializeField] private Transform mTargetTransform = null;
        [Tooltip("Which plane we want to move")]
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_Z;

        private Vector3 mVelocity = Vector3.zero;

        [Header("Move Effect")]
        [SerializeField] private bool mHardOnX = false;
        [SerializeField] private bool mHardOnY = false;
        [SerializeField] private bool mHardOnZ = false;
        //-- Smooth Track
        [Tooltip("Invers of Speed, if smooth track is enable use this.")]
        [SerializeField] private float mMoveFriction = 0.2f;
        //-- Hard Track
        [Tooltip("if smooth track is diable use this.")]
        [SerializeField] private float mMoveSpeed = 10;
        [SerializeField] private float mAccpetRange = 0.8f;

        [Header("Order Effect")]
        [SerializeField] private bool mOrderEffect = false;
        [SerializeField] private Vector3 mGap = Vector3.zero;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float MoveFriction { get { return this.mMoveFriction; } set { this.mMoveFriction = value; } }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public float Index { get { return this.mIndex; } set { this.mIndex = value; } }
        public int OrderIndex { get { return this.mOrderIndex; } set { this.mOrderIndex = value; } }
        public bool Following { get { return this.mFollowing; } set { this.mFollowing = value; } }
        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public void SetTargetTransform(Transform t) { this.mTargetTransform = t; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Update()
        {
            if (mTargetTransform == null)
                mFollowing = false;

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

            Vector3 targetPos = mTargetTransform.position;

            // use this effect u can shoot up to down!
            // if i set the index correctly!
            if (mOrderEffect)
                targetPos += mGap * mIndex;

            Vector3 newPos = this.transform.position;

            SmoothTrack(ref targetPos, ref newPos);
            HardTrack(ref targetPos, ref newPos);
            

           // apply force
            newPos += mVelocity * Time.deltaTime;

            this.transform.position = newPos;
        }

        private void KeepOnSameDirection()
        {
            this.transform.position += mVelocity * Time.deltaTime;
        }

        private void SmoothTrack(ref Vector3 targetPos, ref Vector3 newPos)
        {

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    mVelocity.x = 0;
                    mVelocity.y = (targetPos.y - newPos.y) / mMoveFriction;
                    mVelocity.z = (targetPos.z - newPos.z) / mMoveFriction;
                    break;
                case JCS_Axis.AXIS_Y:

                    mVelocity.x = (targetPos.x - newPos.x) / mMoveFriction;
                    mVelocity.y = 0;
                    mVelocity.z = (targetPos.z - newPos.z) / mMoveFriction;
                    break;
                case JCS_Axis.AXIS_Z:
                    mVelocity.x = (targetPos.x - newPos.x) / mMoveFriction;
                    mVelocity.y = (targetPos.y - newPos.y) / mMoveFriction;
                    mVelocity.z = 0;
                    break;
            }
        }
        private void HardTrack(ref Vector3 targetPos, ref Vector3 newPos)
        {

            if (mHardOnX)
            {
                if (targetPos.x < newPos.x - mAccpetRange)
                    mVelocity.x = -mMoveSpeed;
                else if (targetPos.x > newPos.x + mAccpetRange)
                    mVelocity.x = mMoveSpeed;
                else
                {
                    mVelocity.x = 0;
                }
            }

            if (mHardOnY)
            {
                if (targetPos.y < newPos.y - mAccpetRange)
                    mVelocity.y = -mMoveSpeed;
                else if (targetPos.y > newPos.y + mAccpetRange)
                    mVelocity.y = mMoveSpeed;
                else
                {
                    mVelocity.y = 0;
                }
            }

            if (mHardOnZ)
            {
                if (targetPos.z < newPos.z - mAccpetRange)
                    mVelocity.z = -mMoveSpeed;
                else if (targetPos.z > newPos.z + mAccpetRange)
                    mVelocity.z = mMoveSpeed;
                else
                {
                    mVelocity.z = 0;
                }
            }
        }


    }
}
