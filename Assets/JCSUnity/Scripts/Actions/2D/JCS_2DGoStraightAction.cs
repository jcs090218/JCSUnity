/**
 * $File: JCS_2DGoStraightAction.cs $
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
    public class JCS_2DGoStraightAction
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private float mMoveSpeed = 10.0f;
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void FixedUpdate()
        {
            Vector3 newPos = Vector3.zero;

            switch (mAxis)
            {

                case JCS_Axis.AXIS_X:
                    {
                        newPos = Vector3.right * MoveSpeed * Time.deltaTime;
                    }
                    break;
                case JCS_Axis.AXIS_Z:
                    {
                        newPos = Vector3.forward * MoveSpeed * Time.deltaTime;
                    }
                    break;
                case JCS_Axis.AXIS_Y:
                    {
                        newPos = Vector3.up * MoveSpeed * Time.deltaTime;
                    }
                    break;
            }

            // if is valid, do action.
            if (!JCS_Mathf.IsNaN(newPos))
                this.transform.Translate(newPos);
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
