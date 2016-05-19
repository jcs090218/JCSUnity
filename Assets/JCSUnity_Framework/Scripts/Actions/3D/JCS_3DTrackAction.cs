/**
 * $File: JCS_3DTrackAction.cs $
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

    public class JCS_3DTrackAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [Tooltip("Transform we want to target")]
        [SerializeField] private Transform mTargetTransform = null;
        [Tooltip("Invers of Speed")]
        [SerializeField] private float mMoveFriction = 1;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {

        }

        private void Update()
        {
            FollowObject();
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

            newPos += (targetPos - newPos) / mMoveFriction * Time.deltaTime;

            this.transform.position = newPos;
        }

    }
}
