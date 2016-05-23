/**
 * $File: JCS_SlideEffect.cs $
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

    public class JCS_SlideEffect
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Initialize Variables **")]
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_X;
        [SerializeField] private float mDistance = 10;

        private bool mEffect = false;

        [SerializeField] private float mFriection = 12;
        [SerializeField] private Vector3 mTargetPosition = Vector3.zero;

        [SerializeField] private Vector3 mRecordPosition = Vector3.zero;
        [SerializeField] private Vector3 mTowardPosition = Vector3.zero;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            Vector3 newPos = this.transform.localPosition;
            // record the original position
            this.mRecordPosition = newPos;
            this.mTargetPosition = newPos;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    newPos.x += mDistance;
                    break;
                case JCS_Axis.AXIS_Y:
                    newPos.y += mDistance;
                    break;
                case JCS_Axis.AXIS_Z:
                    newPos.z += mDistance;
                    break;
            }

            this.mTowardPosition = newPos;
        }

        private void Update()
        {
            SlideEffect();

            JCS_OnMouseExit();
        }

        public void JCS_OnMouseOver()
        {
            mEffect = true;
            mTargetPosition = mTowardPosition;
        }
        public void JCS_OnMouseExit()
        {
            if (!mEffect)
                return;

            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                UpdateUnityData();
                if (JCS_UsefualFunctions.MouseOverGUI(this.mRectTransform))
                    return;
            }

            mEffect = false;
            mTargetPosition = mRecordPosition;
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
        private void SlideEffect()
        {
            this.transform.localPosition += (mTargetPosition - transform.localPosition) / mFriection * Time.deltaTime;
        }

    }
}
