/**
 * $File: JCS_SimpleTrackAction.cs $
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
        [SerializeField] private float mFriction = 0.2f;
        [SerializeField] private Vector3 mTargetPos = Vector3.zero;

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
            this.LocalPosition += (mTargetPos - LocalPosition) / mFriction * Time.deltaTime;
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

    }
}
