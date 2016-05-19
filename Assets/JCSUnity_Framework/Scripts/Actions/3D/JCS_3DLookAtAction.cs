/**
 * $File: JCS_3DLookAtAction.cs $
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

    public class JCS_3DLookAtAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private Transform mTargetTransform = null;
        [SerializeField] private JCS_Vector3Direction mLookDirection = JCS_Vector3Direction.UP;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public void SetTargetTransform(Transform t) { this.mTargetTransform = t; }

        //========================================
        //      Unity's function
        //------------------------------

        private void Update()
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

            transform.LookAt(mTargetTransform, direction);
        }

    }
}
