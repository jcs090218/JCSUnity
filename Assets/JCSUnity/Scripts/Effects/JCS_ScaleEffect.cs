/**
 * $File: JCS_ScaleEffect.cs $
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

    public class JCS_ScaleEffect
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Initialize Variables **")]
        [Tooltip("Do scale in x axis.")]
        [SerializeField] private bool mScaleX = true;
        [Tooltip("Do scale in y axis.")]
        [SerializeField] private bool mScaleY = true;
        [Tooltip("Do scale in z axis.")]
        [SerializeField] private bool mScaleZ = true;

        [Tooltip("How much it scale on each axis.")]
        [SerializeField] private Vector3 mScaleValue = Vector3.one;

        [Tooltip("How fast it scale on each axis.")]
        [SerializeField] private Vector3 mScaleFriction = new Vector3(0.2f, 0.2f, 0.2f);

        private Vector3 mTowardScale = Vector3.zero;
        private Vector3 mRecordScale = Vector3.zero;
        private Vector3 mTargetScale = Vector3.zero;

        private bool mEffect = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public Vector3 RecordScale { get { return this.mRecordScale; } set { this.mRecordScale = value; } }
        public Vector3 TowardScale { get { return this.mTowardScale; } set { this.mTowardScale = value; } }
        public Vector3 GetScaleValue() { return this.mScaleValue; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            Vector3 currentScale = this.transform.localScale;

            // record down the scale.
            mRecordScale = currentScale;
            mTargetScale = currentScale;

            SetTargetScale();
        }

        private void Update()
        {
            ScaleEffect();
            JCS_OnMouseExit();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void JCS_OnMouseOver()
        {
            mEffect = true;
            mTargetScale = mTowardScale;
        }

        public void JCS_OnMouseExit()
        {
            if (!mEffect)
                return;

            if (GetObjectType() == JCS_UnityObjectType.UI)
            {
                UpdateUnityData();
                if (JCS_Utility.MouseOverGUI(this.mRectTransform))
                    return;
            }

            mEffect = false;
            mTargetScale = mRecordScale;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void SetTargetScale()
        {
            // so it will scale base on the starting scale.
            Vector3 newTargetScale = mRecordScale;

            if (mScaleX)
            {
                newTargetScale.x += mScaleValue.x;
            }

            if (mScaleY)
            {
                newTargetScale.y += mScaleValue.y;
            }

            if (mScaleZ)
            {
                newTargetScale.z += mScaleValue.z;
            }

            // set to target scale.
            mTowardScale = newTargetScale;
        }

        private void ScaleEffect()
        {
            Vector3 newScale = this.transform.localScale;

            newScale.x += (mTargetScale.x - newScale.x) / mScaleFriction.x * Time.deltaTime;
            newScale.y += (mTargetScale.y - newScale.y) / mScaleFriction.y * Time.deltaTime;
            newScale.z += (mTargetScale.z - newScale.z) / mScaleFriction.y * Time.deltaTime;

            this.transform.localScale = newScale;
        }

    }
}
