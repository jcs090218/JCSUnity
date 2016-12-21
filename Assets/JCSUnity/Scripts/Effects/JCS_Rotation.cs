/**
 * $File: JCS_Rotation.cs $
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
    public class JCS_Rotation
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_Rotation) **")]

        [Tooltip("Do the effect?")]
        [SerializeField]
        protected bool mEffect = false;

        [Tooltip("How fast u rotate?")]
        [SerializeField] [Range(-1000, 1000)]
        protected float mRotateSpeed = 10;

        [Tooltip("Direction u rotate.")]
        [SerializeField]
        protected JCS_Vector3Direction mRotateDirection = JCS_Vector3Direction.FORWARD;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public float RotateSpeed { get { return this.mRotateSpeed; } set { this.mRotateSpeed = value; } }
        public JCS_Vector3Direction RotateDirection { get { return this.mRotateDirection; } set { this.mRotateDirection = value; } }

        //========================================
        //      Unity's function
        //------------------------------

        protected virtual void Update()
        {
            if (!Effect)
                return;

            DoRotation();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Stop doing the effect
        /// </summary>
        public void Stop()
        {
            Effect = false;
            this.transform.localEulerAngles = Vector3.zero;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Algorithm do the rotate effect.
        /// </summary>
        private void DoRotation()
        {
            Vector3 rotateDirection = Vector3.zero;

            switch (mRotateDirection)
            {
                case JCS_Vector3Direction.FORWARD:
                    rotateDirection = Vector3.forward;
                    break;
                case JCS_Vector3Direction.BACK:
                    rotateDirection = Vector3.back;
                    break;
                case JCS_Vector3Direction.UP:
                    rotateDirection = Vector3.up;
                    break;
                case JCS_Vector3Direction.DOWN:
                    rotateDirection = Vector3.down;
                    break;
                case JCS_Vector3Direction.RIGHT:
                    rotateDirection = Vector3.right;
                    break;
                case JCS_Vector3Direction.LEFT:
                    rotateDirection = Vector3.left;
                    break;
                default:
                    rotateDirection = Vector3.zero;
                    break;
            }

            transform.Rotate(rotateDirection * mRotateSpeed * Time.deltaTime);
        }

    }
}
