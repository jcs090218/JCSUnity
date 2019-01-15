﻿/**
 * $File: JCS_OneShotObject.cs $
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
    /// One shot object.
    /// </summary>
    public class JCS_OneShotObject 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_OneShotObject) **")]

        [Tooltip("Effect on which axis?")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        [Tooltip("Do the effect on awake event.")]
        [SerializeField]
        private bool mEffectOnAwake = true;

        [Tooltip("Loop one shot?")]
        [SerializeField]
        private bool mLoop = false;

        [Tooltip("Force in the last position.")]
        [SerializeField]
        private bool mStayAtLastPosition = false;

        [Tooltip("How fast it moves.")]
        [SerializeField]
        private float mMoveSpeed = 5.0f;

        [Tooltip("How far it moves.")]
        [SerializeField]
        private float mDistance = 100.0f;

        private bool mEffect = false;

        private Vector3 mRecordPosition = Vector3.zero;
        private float mDistanceRecorder = 0.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }
        public bool EffectOnAwake { get { return this.mEffectOnAwake; } set { this.mEffectOnAwake = value; } }
        public bool Loop { get { return this.mLoop; } set { this.mLoop = value; } }
        public bool StayAtLastPosition { get { return this.mStayAtLastPosition; } set { this.mStayAtLastPosition = value; } }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public float Distance { get { return this.mDistance; } set { this.mDistance = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            mRecordPosition = this.transform.position;
            mEffect = mEffectOnAwake;

            if (mStayAtLastPosition)
                mLoop = false;
        }

        private void Update()
        {
            if (!mEffect)
                return;

            mDistanceRecorder += JCS_Mathf.AbsoluteValue(mMoveSpeed) * Time.deltaTime;

            if (mDistanceRecorder > mDistance)
            {
                mDistanceRecorder = 0;

                if (!mStayAtLastPosition)
                    this.transform.position = mRecordPosition;

                // if is not loop disable the effect
                if (!mLoop)
                    mEffect = false;
            }


            DoMovement(mAxis, mMoveSpeed);

        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public void PlayOneShot(Vector3 pos)
        {
            PlayOneShot(pos, mDistance);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        public void PlayOneShot(Vector3 pos, float distance)
        {
            mRecordPosition = pos;
            mDistance = distance;
            mEffect = true;
        }
        
        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="speed"></param>
        private void DoMovement(JCS_Axis ax, float speed)
        {
            switch (ax)
            {
                case JCS_Axis.AXIS_X:
                    this.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
                    break;
                case JCS_Axis.AXIS_Y:
                    this.transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
                    break;
                case JCS_Axis.AXIS_Z:
                    this.transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
                    break;
            }
        }

    }
}
