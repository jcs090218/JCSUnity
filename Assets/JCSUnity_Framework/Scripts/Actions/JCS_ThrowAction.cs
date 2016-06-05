/**
 * $File: JCS_ThrowAction.cs $
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
    /// Do the throw action like "Plants vs Zombies"'s 
    /// corn plants.
    /// </summary>
    public class JCS_ThrowAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private bool mEffect = false;

        [Tooltip("Target we are throwing to.")]
        [SerializeField] private Transform mTargetTransform = null;

        private Vector3 mStartingPosition = Vector3.zero;
        private Vector3 mVelocity = Vector3.zero;

        private bool mReachInflectionPoint = false;
        private float mInflectionPoint = 0;
        [Tooltip("Speed of x")]
        [SerializeField] private float mHorizontalFriction = 0.4f; // x & z axis
        [Tooltip("Speed of y")]
        [SerializeField] private float mVerticalForce = 5;        // y axis

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetTargetTransform(Transform trans) { this.mTargetTransform = trans; }

        //========================================
        //      Unity's function
        //------------------------------

        private void LateUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            if (!mEffect)
                return;

            DoThrowEffect();

        }
#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.H))
                ActiveEffect();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void ActiveEffect()
        {
            mStartingPosition = this.transform.localPosition;

            mEffect = true;
            mReachInflectionPoint = false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void DoThrowEffect()
        {
            if (mTargetTransform == null)
                return;

            Vector3 currentPos = this.transform.localPosition;
            mVelocity.x = (this.mTargetTransform.localPosition.x - currentPos.x) / mHorizontalFriction * Time.deltaTime;
            mVelocity.z = (this.mTargetTransform.localPosition.z - currentPos.z) / mHorizontalFriction * Time.deltaTime;

            // before reach to inflection point
            if (!mReachInflectionPoint)
            {
                // calculate inflection point
                {
                    // calcualte the position not the distance.

                    float pointX = ((mTargetTransform.localPosition.x - this.mStartingPosition.x) / 2) + this.mStartingPosition.x;
                    float pointZ = ((mTargetTransform.localPosition.z - this.mStartingPosition.z) / 2) + this.mStartingPosition.z;
                    mInflectionPoint = Mathf.Sqrt(Mathf.Pow(pointX, 2) + Mathf.Pow(pointZ, 2));


                    if (mVelocity.x > 0)
                    {
                        if (currentPos.x >= mInflectionPoint)
                            mReachInflectionPoint = true;
                    }
                    else
                    {
                        if (currentPos.x <= mInflectionPoint)
                            mReachInflectionPoint = true;
                    }
                }

                mVelocity.y = JCS_Mathf.ToPositive((mInflectionPoint - currentPos.x) * mVerticalForce * Time.deltaTime);
            }
            // after reach to inflection point
            else {

                //mVelocity.y = JCS_Mathf.ToNegative(mVerticalForce / (this.mTargetTransform.localPosition.x - currentPos.x) * Time.deltaTime);

                float forceSpeed = JCS_Mathf.ToPositive(mVerticalForce / (this.mTargetTransform.localPosition.x - currentPos.x));
                mVelocity.y = (this.mTargetTransform.localPosition.y - currentPos.y) * forceSpeed * Time.deltaTime;
            }

            // apply new localPosition
            if (!float.IsInfinity(mVelocity.x) &&
                !float.IsInfinity(mVelocity.y) &&
                !float.IsInfinity(mVelocity.z))
                this.transform.localPosition += mVelocity;
        }

    }
}
