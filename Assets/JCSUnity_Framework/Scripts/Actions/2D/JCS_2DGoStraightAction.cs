/**
 * $File: JCS_2DGoStraightAction.cs $
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
        private void Update()
        {
            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    this.transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
                    break;
                case JCS_Axis.AXIS_Z:
                    this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
                    break;
                case JCS_Axis.AXIS_Y:
                    this.transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
                    break;
            }
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
