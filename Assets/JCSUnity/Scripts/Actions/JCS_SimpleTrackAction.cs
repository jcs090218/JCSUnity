/**
 * $File: JCS_SimpleTrackAction.cs $
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
    /// Very simple track action by only passing through the position.
    /// </summary>
    public class JCS_SimpleTrackAction
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_SimpleTrackAction) **")]

        [SerializeField] [Range(0.01f, 10.0f)]
        private float mFriction = 0.2f;

        [SerializeField]
        private Vector3 mTargetPos = Vector3.zero;
        
        [Tooltip("Don't track on x-axis?")]
        [SerializeField]
        private bool mIgnoreX = false;

        [Tooltip("Don't track on y-axis?")]
        [SerializeField]
        private bool mIgnoreY = false;

        [Tooltip("Don't track on z-axis?")]
        [SerializeField]
        private bool mIgnoreZ = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public Vector3 TargetPosition { get { return this.mTargetPos; } set { this.mTargetPos = value; } }
        public float Friction { get { return this.mFriction; } set { this.mFriction = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            UpdateUnityData();
        }

        private void Update()
        {
            Vector3 tempTargetPost = mTargetPos;

            if (mIgnoreX)
                tempTargetPost.x = this.LocalPosition.x;
            if (mIgnoreY)
                tempTargetPost.y = this.LocalPosition.y;
            if (mIgnoreZ)
                tempTargetPost.z = this.LocalPosition.z;

            this.LocalPosition += (tempTargetPost - LocalPosition) / mFriction * Time.deltaTime;
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        public void DeltaTargetPosX(float val)
        {
            mTargetPos = JCS_Utility.IncVecX(mTargetPos, val);
        }

        public void DeltaTargetPosY(float val)
        {
            mTargetPos = JCS_Utility.IncVecY(mTargetPos, val);
        }

        public void DeltaTargetPosZ(float val)
        {
            mTargetPos = JCS_Utility.IncVecZ(mTargetPos, val);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
